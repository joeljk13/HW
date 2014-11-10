﻿using System;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace HW {

    public partial class AssignmentPrompt : Form {

        public AssignmentPrompt(Assignment assign, string name, string due, string time, bool edit)
        {
            assignment = assign;
            InitializeComponent();
            if (edit) {
                AssignName = name;
                Due = due;
                Time = time;
                Load += new EventHandler(AssignmentPrompt_Load);
            }
        }

        private string AssignName = null, Due = null, Time = null;

        private Assignment assignment = null;

        private bool IsOKd = false;

        private void AssignmentPrompt_Load(object sender, EventArgs e)
        {
            Text = "Edit Assignment";
            AssignBox.Text = AssignName;
            DueOptions.Text = Due;
            TimeEstBox.Text = Time;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            IsOKd = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void IsClosing(object sender, FormClosingEventArgs e)
        {
            if (IsOKd) {
                assignment.Replies = new string[] {
                    AssignBox.Text,
                    DueOptions.Text,
                    TimeEstBox.Text
                };
            }
            DialogResult = IsOKd ? DialogResult.OK : DialogResult.Cancel;
        }
    }

    public class Assignment {
        public Assignment(HW form, object sender)
        {
            HWapp = form;
            Sender = sender;
            AssignApp = new Thread(new ThreadStart(Start));
            AssignApp.Start();
        }

        public Assignment(HW form, string name, string due, string time, ToolStripMenuItem subject, int i)
        {
            HWapp = form;
            Name = name;
            Due = due;
            Time = time;
            Subject = subject;
            I = i;
            Edit = true;
            AssignApp = new Thread(new ThreadStart(Start));
            AssignApp.Start();
        }

        private int I = -1;

        private ToolStripMenuItem Subject = null;

        private string Name = "", Due = "Tomorrow", Time = "";

        private bool Edit = false;

        public string[] Replies = null;

        private object Sender = null;

        private Thread AssignApp = null;

        private Form Ap = null;

        private HW HWapp = null;

        private void Load(object sender, EventArgs e)
        {
            HWapp.Visible = false;
        }

        private void Close(object sender, FormClosedEventArgs e)
        {
            // Time est
            HWapp.Visible = true;
            if (Ap.DialogResult == DialogResult.Cancel || Replies == null) {
                return;
            }
            Replies[1] = (new Regex(@"^due\s", RegexOptions.IgnoreCase)).Replace(Replies[1], "");
            Replies[2] = (new Regex(@"^takes\s", RegexOptions.IgnoreCase)).Replace(Replies[2], "");
            if (Replies[0] != "") {
                if (Edit) {
                    // Should this be Replies[1], Replies[2] instead of Due, Time?
                    HWapp.EditAssignment(this, Due, Time, Subject, I);
                }
                else {
                    HWapp.NewAssignment(Replies[0], Replies[1], Replies[2], Sender);
                }
            }
            Ap.Dispose();
        }

        private void Start()
        {
            // Time est
            Ap = new AssignmentPrompt(this, Name, Due, Time, Edit);
            Ap.Load += new EventHandler(Load);
            Ap.FormClosed += new FormClosedEventHandler(Close);
            Application.Run(Ap);
        }
    }

}