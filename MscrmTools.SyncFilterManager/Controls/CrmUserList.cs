using McTools.Xrm.Connection;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using MscrmTools.SyncFilterManager.AppCode;
using MscrmTools.SyncFilterManager.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace MscrmTools.SyncFilterManager.Controls
{
    public partial class CrmUserList : UserControl
    {
        private ConnectionDetail connectionDetail;
        private Panel loadingPanel;
        private string query;
        private List<Entity> savedQueries;
        private Thread searchThread;
        private IOrganizationService service;
        private List<ListViewItem> users;

        public CrmUserList()
        {
            InitializeComponent();

            ToolTip tt = new ToolTip() { IsBalloon = true, ToolTipTitle = "Information" };
            tt.SetToolTip(btnSearch, "Select a Quick search view to use this button");
        }

        public CrmUserList(IOrganizationService service, bool selectMultipleUsers, ConnectionDetail connectionDetail)
        {
            this.service = service;
            this.connectionDetail = connectionDetail;

            InitializeComponent();

            lvUsers.MultiSelect = selectMultipleUsers;
        }

        /// <summary>
        /// EventHandler to request a connection to an organization
        /// </summary>
        public event EventHandler OnRequestConnection;

        public ConnectionDetail ConnectionDetail
        {
            set
            {
                connectionDetail = value;
            }
        }

        [Description("Select Multiple Users"), Category("List")]
        public bool SelectMultipleUsers
        {
            set { lvUsers.MultiSelect = value; }
            get { return lvUsers.MultiSelect; }
        }

        public IOrganizationService Service
        {
            set
            {
                service = value;
                LoadViews();
                FillViewsList();
            }
        }

        public List<Entity> GetSelectedUsers()
        {
            return (from ListViewItem lvi in lvUsers.CheckedItems select (Entity)lvi.Tag).ToList();
        }

        internal void ReplaceUserFilters()
        {
            if (MessageBox.Show(ParentForm,
                "Are you sure you want to apply the selected user synchronization filters to other users?",
                "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            List<Entity> users = null;

            var usDialog = new UserSelectionDialog(service);
            if (usDialog.ShowDialog(this) == DialogResult.OK)
            {
                users = usDialog.SelectedUsers;
            }
            else
            {
                return;
            }

            loadingPanel = InformationPanel.GetInformationPanel(this, "Initiating...", 340, 120);

            var bwReplaceFilters = new BackgroundWorker { WorkerReportsProgress = true };
            bwReplaceFilters.DoWork += bwReplaceFilters_DoWork;
            bwReplaceFilters.ProgressChanged += bwReplaceFilters_ProgressChanged;
            bwReplaceFilters.RunWorkerCompleted += bwReplaceFilters_RunWorkerCompleted;
            bwReplaceFilters.RunWorkerAsync(new object[] { GetSelectedUsers()[0], users });
        }

        internal void Search()
        {
            LoadUsers();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (service == null)
            {
                OnRequestConnection(this, null);
            }
            else
            {
                LoadUsers();
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            QueryExpression qe;
            if (!string.IsNullOrEmpty(query))
            {
                qe = ((FetchXmlToQueryExpressionResponse)service.Execute(new FetchXmlToQueryExpressionRequest
                {
                    FetchXml = query
                })).Query;
            }
            else
            {
                var searchTerm = e.Argument.ToString();

                qe = new QueryExpression("systemuser");
                qe.ColumnSet = new ColumnSet(new[] { "systemuserid", "fullname", "businessunitid" });
                qe.Criteria = new FilterExpression(LogicalOperator.And)
                {
                    Filters =
                {
                    new FilterExpression(LogicalOperator.Or)
                    {
                        Conditions =
                        {
                            new ConditionExpression("fullname", ConditionOperator.BeginsWith,
                                searchTerm.Replace("*", "%")),
                            new ConditionExpression("domainname", ConditionOperator.BeginsWith,
                                searchTerm.Replace("*", "%")),
                            new ConditionExpression("firstname", ConditionOperator.BeginsWith,
                                searchTerm.Replace("*", "%")),
                            new ConditionExpression("lastname", ConditionOperator.BeginsWith,
                                searchTerm.Replace("*", "%"))
                        }
                    },
                    new FilterExpression
                    {
                        Conditions = {new ConditionExpression("isdisabled", ConditionOperator.Equal, false)}
                    }
                }
                };
            }
            qe.PageInfo = new PagingInfo { Count = 250, PageNumber = 1, ReturnTotalRecordCount = true };

            EntityCollection result;
            var results = new List<Entity>();
            InformationPanel.ChangeInformationPanelMessage(loadingPanel, "Retrieving users...");
            do
            {
                result = service.RetrieveMultiple(qe);
                results.AddRange(result.Entities);

                qe.PageInfo.PageNumber++;
            } while (result.MoreRecords);

            e.Result = results;
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //btnSearch.Enabled = true;

            Controls.Remove(loadingPanel);
            loadingPanel.Dispose();

            if (e.Error != null)
            {
                MessageBox.Show(this, "Error while searching users: " + e.Error.Message, "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                users = new List<ListViewItem>();

                foreach (var user in (List<Entity>)e.Result)
                {
                    var lvi = new ListViewItem(user.GetAttributeValue<string>("fullname")) { Tag = user };
                    lvi.SubItems.Add(user.GetAttributeValue<EntityReference>("businessunitid")?.Name);
                    users.Add(lvi);
                }

                DisplayUsers();
            }
        }

        private void bwReplaceFilters_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            var sourceUser = (Entity)((object[])e.Argument)[0];
            var targetUsers = (List<Entity>)((object[])e.Argument)[1];

            var rManager = new RuleManager("userquery", service, connectionDetail);
            rManager.AddRulesFromUser(sourceUser, targetUsers, worker);
        }

        private void bwReplaceFilters_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            InformationPanel.ChangeInformationPanelMessage(loadingPanel, e.UserState.ToString());
        }

        private void bwReplaceFilters_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Controls.Remove(loadingPanel);
            loadingPanel.Dispose();

            if (e.Error != null)
            {
                MessageBox.Show(this, "Error while applying filters: " + e.Error.Message, "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void cbbViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = cbbViews.SelectedItem;
            if (item == null) return;

            if (item is SavedQueryWrapper view)
            {
                var isQuickFind = view.SavedQuery.GetAttributeValue<int>("querytype") == 4;

                btnSearch.Enabled = isQuickFind;
                if (!isQuickFind)
                {
                    query = view.SavedQuery.GetAttributeValue<string>("fetchxml");
                    LoadUsers();
                }
                else
                {
                    query = null;
                }
            }
        }

        private void chkSelectUnselectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvUsers.Items)
            {
                item.Checked = chkSelectUnselectAll.Checked;
            }
        }

        private void CrmUserList_Load(object sender, EventArgs e)
        {
            if (service == null) return;

            var bw = new BackgroundWorker { WorkerReportsProgress = true };
            bw.DoWork += (bw2, evt) =>
            {
                LoadViews();
            };
            bw.RunWorkerCompleted += (bw2, evt) =>
            {
                FillViewsList();
            };
            bw.ProgressChanged += (bw2, evt) => { };
            bw.RunWorkerAsync();
        }

        private void DisplayUsers(object search = null)
        {
            Invoke(new Action(() =>
            {
                lvUsers.Items.Clear();
                lvUsers.Items.AddRange(users.Where(u => search == null || u.Text.IndexOf(search?.ToString(), StringComparison.InvariantCultureIgnoreCase) >= 0).ToArray());
            }));
        }

        private void FillViewsList()
        {
            cbbViews.Items.Clear();
            cbbViews.Items.Add("--- System views ---");
            cbbViews.Items.AddRange(savedQueries.Where(s => s.LogicalName == "savedquery").Select(s => new SavedQueryWrapper(s)).OrderBy(s => s.ToString()).ToArray());
            cbbViews.Items.Add("--- Personal views ---");
            cbbViews.Items.AddRange(savedQueries.Where(s => s.LogicalName == "userquery").Select(s => new SavedQueryWrapper(s)).OrderBy(s => s.ToString()).ToArray());
        }

        private void LoadUsers()
        {
            //btnSearch.Enabled = false;
            lvUsers.Items.Clear();

            loadingPanel = InformationPanel.GetInformationPanel(this, "Retrieving users...", 340, 120);

            var bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.RunWorkerAsync(txtSearch.Text);
        }

        private void LoadViews()
        {
            if (service == null) return;

            savedQueries = service.RetrieveMultiple(new QueryExpression("savedquery")
            {
                NoLock = true,
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression
                {
                    Conditions =
                            {
                                new ConditionExpression("returnedtypecode", ConditionOperator.Equal, "systemuser")
                            }
                }
            }).Entities.ToList();

            savedQueries.AddRange(service.RetrieveMultiple(new QueryExpression("userquery")
            {
                NoLock = true,
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression
                {
                    Conditions =
                            {
                                new ConditionExpression("returnedtypecode", ConditionOperator.Equal, "systemuser")
                            }
                }
            }).Entities.ToList());
        }

        private void lvUsers_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            lvUsers.Sorting = lvUsers.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            lvUsers.ListViewItemSorter = new ListViewItemComparer(e.Column, lvUsers.Sorting);
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                btnSearch_Click(null, null);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (query != null)
            {
                searchThread?.Abort();
                searchThread = new Thread(DisplayUsers);
                searchThread.Start(txtSearch.Text);
            }
        }
    }
}