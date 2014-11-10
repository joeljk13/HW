using System;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace HW
{
    public partial class SubjectPrompt : Form
    {
        public SubjectPrompt()
        {
            InitializeComponent();
        }

        private bool IsOKd = false;

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
            Tag = SubjectBox.Text;
            DialogResult = IsOKd ? DialogResult.OK : DialogResult.Cancel;
        }
    }

    public class Subject
    {
        public Subject(HW form)
        {
            HWapp = form;
            SubApp = new Thread(new ThreadStart(Start));
            SubApp.Start();
        }

        private Thread SubApp = null;

        private Form Sp = null;

        private HW HWapp = null;

        private string Reply = null;

        private void Load(object sender, EventArgs e)
        {
            //HWapp.Visible = false;
        }

        private void Close(object sender, FormClosedEventArgs e)
        {
            //HWapp.Visible = true;
            if (Sp.DialogResult == DialogResult.OK)
                Reply = Sp.Tag.ToString();
            if (Reply == null || Reply == "")
                return;
            HWapp.NewSubject((new Regex(@"^\s+|\s+(?=\s)|\s+$")).Replace(Reply, ""));
            Sp.Dispose();
        }

        private void Start()
        {
            Sp = new SubjectPrompt();
            Sp.Load += new EventHandler(Load);
            Sp.FormClosed += new FormClosedEventHandler(Close);
            Application.Run(Sp);
        }
    }
}
