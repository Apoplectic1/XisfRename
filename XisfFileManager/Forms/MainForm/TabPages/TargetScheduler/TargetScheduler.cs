using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using XisfFileManager.Enums;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace XisfFileManager
{
    public partial class MainForm
    {
        // **********************************************************************************************************************************
        // **********************************************************************************************************************************
        // Target Scheduler Methods
        // **********************************************************************************************************************************
        // **********************************************************************************************************************************

        private void Button_SchedulerTab_OpenDatabase_Click(object sender, EventArgs e)
        {
#if DEBUG
            mSchedulerDB.mSqlReader.ReadTargetSchedulerDataBaseFile(@"E:\Projects\Visual Studio\Astronomy\XisfFileManager\TestData\schedulerdb.sqlite");
#else
            mSchedulerDB.mSqlReader.ReadTargetSchedulerDataBaseFile(@"\\BIRDWATCHER\SchedulerPlugin\schedulerdb.sqlite");
#endif
            TreeView_SchedulerTab_ProfileTree.Nodes.Clear();
            TreeView_SchedulerTab_ProjectTree.Nodes.Clear();
            TreeView_SchedulerTab_PlansTree.Nodes.Clear();

            foreach (var profilePreference in mSchedulerDB.mProfilePreferenceList)
            {
                // Add Profile to Profile Tree
                TreeNode TreeView_SchedulerTab_ProfileTree_RootNode = new TreeNode(profilePreference.profileId.Substring(profilePreference.profileId.LastIndexOf('-') + 5));
                TreeView_SchedulerTab_ProfileTree.Nodes.Add(TreeView_SchedulerTab_ProfileTree_RootNode);

                // Add Projects to the Project Tree
                //TreeNode TreeView_SchedulerTab_ProjectTree_RootNode = new TreeNode(profilePreference.profileId.Substring(profilePreference.profileId.LastIndexOf('-') + 5));
                //TreeView_SchedulerTab_ProjectTree.Nodes.Add(TreeView_SchedulerTab_ProjectTree_RootNode);

                foreach (var project in mSchedulerDB.mProjectList)
                {
                    if (project.profileId == profilePreference.profileId)
                    {
                        TreeNode projectNode = new TreeNode(project.name);
                        TreeView_SchedulerTab_ProjectTree.Nodes.Add(projectNode);
                    }
                }
            }

            TreeView_SchedulerTab_TargetTree.Nodes.Clear();

            foreach (var target in mSchedulerDB.mTargetList)
            {
                TreeNode TreeView_SchedulerTab_TargetTree_RootNode = new TreeNode(target.name);
                TreeView_SchedulerTab_TargetTree.Nodes.Add(TreeView_SchedulerTab_TargetTree_RootNode);
            }

            RefineExposurePlans();

            TreeView_SchedulerTab_ProfileTree.ExpandAll();
            TreeView_SchedulerTab_ProjectTree.ExpandAll();


            // Do Things
            //mSchedulerDB.UpdateTargetImageCounts(mFileList);
        }

        private string ProfilePreference(TreeNode projectNode) { return mSchedulerDB.mProfilePreferenceList.Find(profile => profile.profileId.Contains(projectNode.Parent.Text)).profileId; }
        private bool ProjectState(TreeNode projectNode, string sProfilePreference) { return mSchedulerDB.mProjectList.Any(project => project.name == projectNode.Text && project.profileId == sProfilePreference); }
        private int ProjectPriority(TreeNode projectNode, string sProfilePreference) { return mSchedulerDB.mProjectList.Find(project => (project.name == projectNode.Text) && (project.profileId == sProfilePreference)).priority; }

        private void SetProjectActiveCheckBox(TreeNode projectNode)
        {
            string profilePreference = ProfilePreference(projectNode);
            //CheckBox_Project_Active.Checked = ProjectState(projectNode, profilePreference);
        }

        private void SetProjectPriorityRadioButtons(TreeNode projectNode)
        {
            string profilePreference = ProfilePreference(projectNode);
            int priority = ProjectPriority(projectNode, profilePreference);
            switch (priority)
            {
                case (int)eProjectPriority.LOW:
                    RadioButton_ProjectPriority_Low.Checked = true;
                    break;
                case (int)eProjectPriority.NORMAL:
                    RadioButton_ProjectPriority_Normal.Checked = true;
                    break;
                case (int)eProjectPriority.HIGH:
                    RadioButton_ProjectPriority_High.Checked = true;
                    break;
            }


            RefineSelectedProjectTreeView(projectNode);
        }

        private static void ExpandAllNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.Expand();
                ExpandAllNodes(node.Nodes);
            }
        }

        


        private void RefineSelectedProjectTreeView(TreeNode clickedNode)
        {
            TreeView_SchedulerTab_ProjectTree.Nodes.Clear();
            TreeView_SchedulerTab_TargetTree.Nodes.Clear();

            string profilePreference = ProfilePreference(clickedNode);

            TreeNode TreeView_SchedulerTab_ProjectTree_RootNode = new TreeNode(profilePreference.Substring(profilePreference.LastIndexOf('-') + 5));
            TreeView_SchedulerTab_ProjectTree.Nodes.Add(TreeView_SchedulerTab_ProjectTree_RootNode);

            foreach (var project in mSchedulerDB.mProjectList)
            {
                if (project.profileId == profilePreference)
                {
                    TreeNode projectNode = new TreeNode(project.name);
                    TreeView_SchedulerTab_ProjectTree_RootNode.Nodes.Add(projectNode);
                }
            }

            TreeView_SchedulerTab_ProjectTree.ExpandAll();
        }

        private void RefineSelectedTargetTreeView(TreeNode clickedNode)
        {
            TreeNode profileNode = clickedNode.Parent;

            TreeView_SchedulerTab_TargetTree.Nodes.Clear();

            string projectProfileId = mSchedulerDB.mProjectList.Find(project => project.profileId.Contains(profileNode.Text)).profileId;

            int projectId = mSchedulerDB.mProjectList.Find(project => project.name == clickedNode.Text && project.profileId == projectProfileId).Id;

            string profileId = mSchedulerDB.mProjectList.Find(project => project.profileId == projectProfileId).profileId;

            foreach (var profilePreference in mSchedulerDB.mProfilePreferenceList)
            {
                foreach (var target in mSchedulerDB.mTargetList)
                {
                    if ((projectId == target.projectid) && (projectProfileId == profilePreference.profileId))
                    {
                        TreeNode TreeView_SchedulerTab_TargetTree_RootNode = new TreeNode(target.name);
                        TreeView_SchedulerTab_TargetTree.Nodes.Add(TreeView_SchedulerTab_TargetTree_RootNode);
                    }
                }
            }
        }

        private void RefineExposurePlans()
        {
            TreeView_SchedulerTab_PlansTree.Nodes.Clear();

            foreach (TreeNode targetNode in TreeView_SchedulerTab_TargetTree.Nodes)
            {
                int targetId = mSchedulerDB.mTargetList.Find(target => target.name.Equals(targetNode.Text)).Id;
                string targetName = targetNode.Text;

                List<int> exposureTemplateIdList = mSchedulerDB.mExposurePlanList
                                .Where(plan => plan.targetid == targetId)
                                .Select(plan => plan.exposureTemplateId)
                                .ToList();

                foreach (var plan in exposureTemplateIdList)
                {
                    string exposurePlanName = targetName + " " + mSchedulerDB.mExposureTemplateList.Find(template => template.Id == plan).filtername;
                    TreeNode TreeView_SchedulerTab_PlansTree_RootNode = new TreeNode(exposurePlanName);
                    TreeView_SchedulerTab_PlansTree.Nodes.Add(TreeView_SchedulerTab_PlansTree_RootNode);
                }
            }
        }

        // **********************************************************************************************************************************
        // Event Handlers
        // **********************************************************************************************************************************

        private void TreeView_SchedulerTab_ProfileTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode clickedNode = e.Node;

            string clickedItem = clickedNode.Text;
            MessageBox.Show($"You clicked on: {clickedItem}");
        }

        private void TreeView_SchedulerTab_ProjectTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode clickedNode = e.Node;

            if (clickedNode.Parent == null)
                // We clicked on a Profile
                RefineSelectedProjectTreeView(clickedNode);
            else
            {
                // We clicked on a Project
                SetProjectActiveCheckBox(clickedNode);
                SetProjectPriorityRadioButtons(clickedNode);

                RefineSelectedTargetTreeView(clickedNode);
            }

            RefineExposurePlans();
        }

        private void TreeView_SchedulerTab_PlanTree_NodeMouseClick_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode clickedNode = e.Node;
            if (clickedNode == null)
                return;

            string clickedItem = clickedNode.Text;
            MessageBox.Show($"You clicked on: {clickedItem}");
        }

        private void TreeView_SchedulerTab_TargetTree_NodeMouseClick_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode clickedNode = e.Node;
            if (clickedNode == null)
                return;

            TreeNode targetNodeParent = clickedNode.Parent;

            RefineExposurePlans();
        }

        private void TreeView_SchedulerTab_ProjectTree_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = (sender as TreeView).SelectedNode;
            if (selectedNode == null)
                return;
        }

        private void TreeView_SchedulerTab_ProjectTree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            DrawTreeNodeEventArgs args = e;
            if (args == null)
                return;

            if (args.Node.Parent == null)
            {
                args.DrawDefault = true;
                return;
            }
            string profilePreference = ProfilePreference(args.Node);


            int priority = ProjectPriority(args.Node, profilePreference);
            switch (priority)
            {
                case (int)eProjectPriority.LOW:
                    e.Graphics.DrawString(args.Node.Text, DefaultFont, new SolidBrush(Color.SandyBrown),
                          new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
                    //e.Graphics.DrawString(args.Node.Text, DefaultFont, Brushes.SandyBrown, args.Bounds);
                    break;
                case (int)eProjectPriority.NORMAL:
                    e.Graphics.DrawString(args.Node.Text, DefaultFont, Brushes.Black, args.Bounds);
                    break;
                case (int)eProjectPriority.HIGH:
                    e.Graphics.DrawString(args.Node.Text, DefaultFont, Brushes.DarkMagenta, args.Bounds);
                    break;
            }
        }

    }
}
