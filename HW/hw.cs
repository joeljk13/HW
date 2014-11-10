#define FADE_OUT
#define NO_RESET_ON_ERROR
#define NO_SUPPRESS_STARTUP_MSGBXS

using System;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace HW
{
    public partial class HW : Form
    {
        private static void Main()
        {
            Application.Run(new HW());
        }

        public HW()
        {
            InitializeComponent();
        }

        private string Title = "";

        private void HW_Load(object sender, EventArgs E)
        {
            try
            {
                InitiateMenu();
                InitiateConsole();
                UpdateXml();
            }
            catch (Exception e)
            {
                MessageBox.Show("There was an error loading the application.\n\n" + e.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                HasError = true;
                Close();
            }
        }

        private Thread ConsoleThread = null;

        private void InitiateConsole()
        {
            Title = Console.Title;
            Console.Title = "Home Work - Commands";
            SetConsole();
            ConsoleThread = new Thread(new ParameterizedThreadStart(ReadConsole));
            ConsoleThread.Start(null);
        }

        private FileInfo FindFile(string name, DirectoryInfo dir)
        {
            foreach (FileInfo file in dir.GetFiles())
                if (file.Name == name)
                    return file;
            foreach (DirectoryInfo direc in dir.GetDirectories())
            {
                FileInfo file = FindFile(name, direc);
                if (file != null)
                    return file;
            }
            return null;
        }

        private bool StartProcess(string name, DirectoryInfo dir, string args = "")
        {
            try
            {
                FileInfo open = FindFile(name, dir);
                // Any idea what I was thinking here?
                // if (!open.Exists)
                //     open = FindFile(name, dir);
                if (open.Exists)
                    Process.Start(open.FullName, args);
                else return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SetConsole()
        {
            Console.Clear();
            Console.WriteLine("Type any commands here.");
        }

        private DateTime LastVisit = DateTime.Now;

        private Regex
            // For if an assignment is due in a while
            Due = new Regex(@"^due\s\d(\d?)(-|\/)\d(\d?)((\2\d\d(\d\d?))?)$",
                RegexOptions.IgnoreCase),
            // For how long an assignment takes
            Takes = new Regex(@"^takes\s\d(\d?)\:\d\d$", RegexOptions.IgnoreCase);

        private void ReadConsole(object defLine)
        {
            try
            {
                string line = (string)defLine;
                if (defLine == null)
                    line = Console.ReadLine();
                line = Trim(line).ToLower();
                string cmd = line.Split(' ')[0], det;
                if (line.IndexOf(' ') < 0)
                    det = "";
                else det = line.Substring(line.IndexOf(' ') < 0 ? 0 : line.IndexOf(' ') + 1);
                if (cmd == "clear")
                {
                    SetConsole();
                    ReadConsole(null);
                    return;
                }
                else if (cmd == "date")
                    Console.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now
                        .ToShortTimeString());
                else if (cmd == "time")
                {
                    TimeSpan total = TimeSpan.Zero;
                    foreach (ToolStripMenuItem subject in menu.Items)
                    {
                        if (subject.Equals(menu.Items[0]))
                            continue;
                        foreach (ToolStripMenuItem assignment in subject.DropDownItems)
                        {
                            if (assignment.DropDownItems.Count < 3)
                                continue;
                            string timeTxt = assignment.DropDownItems[0].Text;
                            if (!Takes.IsMatch(timeTxt))
                            {
                                timeTxt = assignment.DropDownItems[1].Text;
                                if (!Takes.IsMatch(timeTxt))
                                    continue;
                            }
                            string[] times = timeTxt.Substring(6).Split(':');
                            total.Add(new TimeSpan(Int32.Parse(times[0]), Int32.Parse(times[1]),
                                0));
                        }
                    }
                    Console.WriteLine(total.Hours + ":" + (total.Minutes < 10 ? "0" : "") +
                        total.Minutes);
                }
                else if (cmd == "list")
                {
                    if (det == "short term" || det == "st" || det == "dt")
                        det = "due tomorrow";
                    else if (det == "long term" || det == "lt" || det == "dw")
                        det = "due in a while";
                    if (det.Length > 4 && det.Remove(4) == "due ")
                        foreach (ToolStripMenuItem subject in menu.Items)
                        {
                            if (subject.Equals(menu.Items[0]))
                                continue;
                            bool sub = false;
                            foreach (ToolStripMenuItem assignment in subject.DropDownItems)
                                if (assignment.DropDownItems.Count >= 3)
                                {
                                    string txt = assignment.DropDownItems[0].Text;
                                    if (!txt.StartsWith("Due"))
                                    {
                                        txt = assignment.DropDownItems[1].Text;
                                        if (!txt.StartsWith("Due"))
                                            continue;
                                    }
                                    if (txt.ToLower() == det)
                                    {
                                        string time = assignment.DropDownItems[1].Text;
                                        Console.Write((sub ? "; " : subject.Text + ": ") +
                                            assignment.Text + (assignment.DropDownItems.Count == 4 ?
                                            (" (" + assignment.DropDownItems[1].Text + ")") :
                                            ""));
                                        sub = true;
                                    }
                                    else if (det == "due in a while" && txt.ToLower() !=
                                        "due tomorrow")
                                    {
                                        Console.Write((sub ? "; " : (subject.Text + ": ")) +
                                            assignment.Text + " - " + assignment.DropDownItems[0]
                                            .Text + (assignment.DropDownItems.Count == 4 ?
                                            (" (" + assignment.DropDownItems[1].Text + ")") :
                                            ""));
                                        sub = true;
                                    }
                                }
                            if (sub)
                            {
                                Console.WriteLine();
                                Console.WriteLine();
                            }
                        }
                    else if (det == "not due")
                        foreach (ToolStripMenuItem subject in menu.Items)
                        {
                            if (subject.Equals(menu.Items[0]))
                                continue;
                            bool sub = false;
                            foreach (ToolStripMenuItem assignment in subject.DropDownItems)
                                if (assignment.DropDownItems.Count == 2 || (assignment
                                    .DropDownItems.Count == 3 && Takes.IsMatch(assignment
                                    .DropDownItems[0].Text)))
                                {
                                    Console.Write((sub ? "; " : subject.Text + ": ") +
                                        assignment.Text + (assignment.DropDownItems.Count == 3 ?
                                        (" (" + assignment.DropDownItems[0].Text + ")") : ""));
                                    sub = true;
                                }
                            if (sub)
                            {
                                Console.WriteLine();
                                Console.WriteLine();
                            }
                        }
                    else
                    {
                        bool sub = false;
                        foreach (ToolStripMenuItem subject in menu.Items)
                        {
                            if (det == subject.Text.ToLower())
                            {
                                sub = true;
                                break;
                            }
                        }
                        foreach (ToolStripMenuItem subject in menu.Items)
                        {
                            if (subject.Equals(menu.Items[0]))
                                continue;
                            if (sub && det == subject.Text.ToLower())
                            {
                                sub = false;
                                foreach (ToolStripMenuItem assignment in subject.DropDownItems)
                                {
                                    if (assignment.DropDownItems.Count == 0)
                                        continue;
                                    if (sub)
                                        Console.Write("; ");
                                    Console.Write(assignment.Text);
                                    if (assignment.DropDownItems.Count == 3)
                                        Console.Write((assignment.DropDownItems[0].Text
                                            .StartsWith("Due")) ? (" - " + assignment
                                            .DropDownItems[0].Text) : (" (" + assignment
                                            .DropDownItems[0].Text + ")"));
                                    else if (assignment.DropDownItems.Count == 4)
                                        Console.Write(" - " + assignment.DropDownItems[0].Text +
                                            " (" + assignment.DropDownItems[1].Text + ")");
                                    sub = true;
                                }
                                if (!sub)
                                    Console.WriteLine("No assignments.");
                                break;
                            }
                            else if (!sub)
                            {
                                Console.Write(subject.Text + ": ");
                                sub = false;
                                foreach (ToolStripMenuItem assignment in subject.DropDownItems)
                                {
                                    if (assignment.DropDownItems.Count == 0)
                                        continue;
                                    if (sub)
                                        Console.Write("; ");
                                    Console.Write(assignment.Text);
                                    if (assignment.DropDownItems.Count == 3)
                                        Console.Write((assignment.DropDownItems[0].Text.Contains(
                                            "Due") ? (" - " + assignment.DropDownItems[0].Text) :
                                            (" (" + assignment.DropDownItems[1].Text + ")")));
                                    sub = true;
                                }
                                if (subject.DropDownItems.Count == 2)
                                    Console.Write("No assignments");
                                Console.WriteLine();
                                Console.WriteLine();
                            }
                        }
                    }
                }
                else if (cmd == "count")
                {
                    Console.WriteLine("Assignments due tomorrow: {0}", ShortTerm);
                    Console.WriteLine("Assignments due in a while: {0}", LongTerm);
                }
                else if (cmd == "save")
                    UpdateXml();
                else if (cmd == "update")
                {
                    UpdateXml();
                    SetConsole();
                    if (det == "")
                        det = "st";
                    ReadConsole("list " + det);
                }
                else if (cmd == "reset")
                    ResetXml();
                else if (cmd == "close" || cmd == "exit" || cmd == "quit")
                    Close();
                else if (cmd == "open" || cmd == "show")
                {
                    DirectoryInfo desktop = new DirectoryInfo(Environment.GetFolderPath(
                            Environment.SpecialFolder.DesktopDirectory));
                    if (det == "hw" || det == "home work")
                    {
                        if (WindowState == FormWindowState.Minimized)
                            WindowState = FormWindowState.Normal;
                        BringToFront();
                    }
                    else if (det.StartsWith("timer"))
                    {
                        string dir = "C:/Home/Joel/Programs/C++/Timer/Release/";
                        if (!StartProcess("Timer.exe", new DirectoryInfo(dir), det.Length > 5 ? det.Substring(5) : ""))
                            Console.WriteLine("Couldn't find Timer.exe in " + dir);
                    }
                    else if (det == "word" || det == "word mla")
                    {
                        if (!StartProcess(det + ".doc", desktop))
                            if (!StartProcess(det + ".docx", desktop))
                                Console.WriteLine("No \"" + det + ".doc\" or \"" + det +
                                    ".docx\" was found.");
                    }
                    else if (det == "excel" || det == "excel hw")
                    {
                        if (!StartProcess(det + ".xls", desktop))
                            if (!StartProcess(det + ".xlsx", desktop))
                                Console.WriteLine("No \"" + det + ".xls\" or \"" + det +
                                    ".xlsx\" was found.");
                    }
                    else if (File.Exists(det) || (new Regex(
                        @"^((http\:\/\/?)(www\.?)[-\w_\d]+\.[a-zA-Z]{2}.*)$", RegexOptions
                        .IgnoreCase)).IsMatch(det == null ? "" : det))
                        Process.Start(det);
                    else if (det == "")
                        Console.WriteLine("You need to specify something to {0}.", cmd);
                    else Console.WriteLine("This program cannot {0} what you specified.", cmd);
                }
                else if (cmd == "play" && (new Regex(
                    @"^(tone\s((a|b|c|d|e|f|g)((\s?)(\#|b)?)|\d+)((\sfor\s\d+)?))$"))
                    .IsMatch(det))
                {
                    int ms = 0, hz = 0;
                    if ((new Regex(@"tone\s\d+")).IsMatch(det))
                        try
                        {
                            hz = Int32.Parse((new Regex(@"\d+")).Match(det).Value);
                        }
                        catch { }
                    if (det.Contains("for"))
                        try
                        {
                            ms = Int32.Parse((new Regex(@"r\s\d+")).Match(det).Value.Substring(2));
                        }
                        catch { }
                    if ((hz < 37 && hz != 0) || hz > 32767 || (ms < 1 && ms != 0) || ms > 60000)
                        Console.WriteLine("A measurement was out of range.");
                    else
                    {
                        if (hz == 0)
                            if (new Regex(@"tone\s(a|b|c|d|e|f|g)").IsMatch(det))
                            {
                                string note = (new Regex(@"ne\s(a|b|c|d|e|f|g)((\s?)(\#|b)?)"))
                                    .Match(det).Value.Substring(3).Replace(" ", "");
                                if (note == "a#" || note == "bb")
                                    hz = 466;
                                else if (note == "b" || note == "cb")
                                    hz = 494;
                                else if (note == "b#" || note == "c")
                                    hz = 262;
                                else if (note == "c#" || note == "db")
                                    hz = 277;
                                else if (note == "d")
                                    hz = 294;
                                else if (note == "d#" || note == "eb")
                                    hz = 311;
                                else if (note == "e" || note == "fb")
                                    hz = 330;
                                else if (note == "e#" || note == "f")
                                    hz = 349;
                                else if (note == "f#" || note == "gb")
                                    hz = 370;
                                else if (note == "g")
                                    hz = 392;
                                else if (note == "g#" || note == "ab")
                                    hz = 415;
                                else hz = 440;
                            }
                            else hz = 440;
                        if (ms == 0)
                            ms = 1000;
                        try
                        {
                            Console.Beep(hz, ms);
                        }
                        catch { }
                    }
                }
                else if (cmd == "fizzbuzz")
                {
                    for (int i = 1; i < 101; i++)
                    {
                        string write = "";
                        if (i % 5 == 0)
                            write += "Fizz";
                        if (i % 3 == 0)
                            write += "Buzz";
                        if (write == "")
                            Console.WriteLine(i);
                        else Console.WriteLine(write);
                    }
                }
                else if (cmd == "new")
                {
                    // TODO?
                    if (det == "sub" || det == "subject")
                        NewSubject(null, null);
                    else if ((new Regex(@"^[\w\d\s-]+\sassignment$", RegexOptions.IgnoreCase))
                        .IsMatch(det))
                    {
                        string subjectTxt = det.Remove(det.LastIndexOf(' ')).ToLower();
                        foreach (ToolStripMenuItem subject in menu.Items)
                        {
                            if (!subject.Equals(menu.Items[0]) && subject.Text.ToLower() ==
                                subjectTxt)
                            {
                                NewAssignment(subject, null);
                                break;
                            }
                        }
                    }
                    else Console.WriteLine("This program cannot create what you specified.");
                }
                else if (cmd == "del" || cmd == "delete")
                {
                    // TODO
                    Console.WriteLine("This feature has not been implemented yet.");
                }
                else if (cmd != "")
                    Console.WriteLine("That is not a reconized command.");
                ClearConsole();
                ReadConsole(null);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                ConsoleThread.Abort();
            }
        }

        private void ClearConsole()
        {
            Console.WriteLine();
            for (int i = 0, max = 10; i < max; i++)
                Console.Write("-----");
            Console.WriteLine();
            Console.WriteLine();
        }

        private void FindKeyCombos(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q && e.Modifiers.HasFlag(Keys.Control))
            {
                ExitKeyPressed = true;
                Close();
            }
            else if (e.KeyCode == Keys.S && e.Modifiers.HasFlag(Keys.Control))
                UpdateXml();
        }

        private bool ExitKeyPressed = false, HasError = false;

        private void HW_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (HasError)
                return;
            if (ExitKeyPressed)
            {
#if FADE_OUT
                FadeOut(Opacity);
#endif
                return;
            }
            if (ShortTerm > 0)
            {
                MessageBox.Show("There is still home work due tomorrow.", "Not finished",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
            else if (LongTerm > 0)
                e.Cancel = MessageBox.Show("There is still some home work that has not " +
                    "been done.\nAre you sure you want to exit?", "Not finished",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel;
            if (e.Cancel)
                UpdateXml();
#if FADE_OUT
            else
                FadeOut(Opacity);
#endif
        }

#if FADE_OUT
        private void FadeOut(double op)
        {
            if (op > 0 && WindowState == FormWindowState.Normal)
            {
                Opacity = op;
                Thread.Sleep(1);
                double sub = Math.Sqrt(Width * Width + Height * Height) / 200;
                FadeOut(op - (sub / 100 < .01 ? .01 : sub / 100));
            }
        }
#endif

        private void HW_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (!HasError)
                {
                    LastVisit = DateTime.Now;
                    UpdateXml();
                }
#if RESET_ON_ERROR
                else ResetXml();
#endif
                if (ConsoleThread != null)
                {
                    if (ConsoleThread.IsAlive)
                        ConsoleThread.Abort();
                    Console.Clear();
                }
                this.Dispose(true);
                Environment.Exit(0);
            }
            catch { }
        }

        private void ResetXml()
        {
            try
            {
                if (!File.Exists("menu.xml"))
                    File.Create("menu.xml");
                TextWriter writer = new StreamWriter("menu.xml", false);
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                writer.WriteLine("<menu>");
                writer.WriteLine("  <item text=\"Tools\">");
                writer.WriteLine("    <item text=\"New subject\" class=\"new-subject\">");
                writer.WriteLine("    </item>");
                writer.WriteLine("    <item text=\"Delete empty subjects\" class=\"" +
                    "delete-empty-subjects\">");
                writer.WriteLine("    </item>");
                writer.WriteLine("  </item>");
                writer.Write("</menu>");
                writer.Close();
            }
            catch { }
        }

        private MenuStrip menu = null;

        private void UpdateXml()
        {
            try
            {
                string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<menu>";
                xml += "\n  <shortterm count=\"" + ShortTerm + "\" />";
                xml += "\n  <longterm count=\"" + LongTerm + "\" />";
                xml += "\n  <lastvisit date=\"" + LastVisit.ToShortDateString() + " " + LastVisit
                    .ToShortTimeString() + "\" />";
                foreach (ToolStripMenuItem subject in menu.Items)
                {
                    xml += "\n  <item text=\"" + subject.Text + "\"";
                    if (subject.Tag != null)
                        xml += " class=\"" + subject.Tag.ToString() + "\"";
                    xml += ">";
                    foreach (ToolStripMenuItem assignment in subject.DropDownItems)
                    {
                        xml += "\n    <item text=\"" + assignment.Text + "\"";
                        if (assignment.Tag != null && assignment.Enabled)
                            xml += " class=\"" + assignment.Tag.ToString() + "\"";
                        else if (assignment.Tag != null && !assignment.Enabled)
                            xml += " class=\"" + assignment.Tag.ToString() + " disabled\"";
                        else if (!assignment.Enabled)
                            xml += " class=\"disabled\"";
                        xml += ">";
                        foreach (ToolStripMenuItem info in assignment.DropDownItems)
                        {
                            xml += "\n      <item text=\"" + info.Text + "\"";
                            if (info.Tag != null && info.Enabled)
                                xml += " class=\"" + info.Tag.ToString() + "\"";
                            else if (info.Tag != null && !info.Enabled)
                                xml += " class=\"" + info.Tag.ToString() + " disabled\"";
                            else if (!info.Enabled)
                                xml += " class=\"disabled\"";
                            xml += "></item>";
                        }
                        xml += "\n    </item>";
                    }
                    xml += "\n  </item>";
                }
                xml += "\n</menu>";
                TextWriter writer = new StreamWriter("menu.xml", false);
                writer.Write(xml);
                writer.Close();
            }
            catch
            {
#if RESET_ON_ERROR
                ResetXml();
#endif
            }
        }

        private bool HasClass(XmlNode node, string className)
        {
            try
            {
                foreach (string Class in Trim(node.Attributes["class"].Value).Split(' '))
                    if (Class.ToLower() == className.ToLower())
                        return true;
            }
            catch { }
            return false;
        }

        private string Trim(string Text)
        {
            return (new Regex(@"^\s+|\s+(?=\s)|\s+$")).Replace(Text, "");
        }

        private XmlDocument Xml = null;

        private void InitiateMenu()
        {
            Xml = new XmlDocument();
            Xml.Load("menu.xml");
            if (Xml.DocumentElement.Name != "menu")
                throw new XmlException("menu.xml has an invalid root element.");
            menu = new MenuStrip();
            menu.Stretch = true;
            menu.BackColor = SystemColors.ScrollBar;
            menu.Top = 0;
            ToolStripMenuItem item1 = null, item2 = null, item3 = null;
            Regex noDis = new Regex(@"(^disabled(\s?))|(\sdisabled)", RegexOptions.IgnoreCase);
            foreach (XmlNode node1 in Xml.DocumentElement.ChildNodes)
            {
                try
                {
                    if (node1.Name == "shortterm")
                        ShortTerm = Int32.Parse(Trim(node1.Attributes["count"].Value));
                    else if (node1.Name == "longterm")
                        LongTerm = Int32.Parse(Trim(node1.Attributes["count"].Value));
                    else if (node1.Name == "lastvisit")
                    {
                        string visit = Trim(node1.Attributes["date"].Value);
                        string[] date = visit.Split(' ')[0].Split('/'), time = visit.Substring(
                            visit.IndexOf(' ') + 1).Split(' ')[0].Split(':');
                        LastVisit = new DateTime(Int32.Parse(date[2]), Int32.Parse(date[0]), Int32
                            .Parse(date[1]), Int32.Parse(time[0]) + (visit.Contains("PM") ? 12 :
                            0), Int32.Parse(time[1]), 0);
                    }
                }
                catch { }
                if (node1.Name != "item")
                    continue;
                try
                {
                    item1 = new ToolStripMenuItem(Trim(node1.Attributes["text"].Value));
                }
                catch
                {
                    continue;
                }
                foreach (XmlNode node2 in node1.ChildNodes)
                {
                    if (node2.Name != "item")
                        continue;
                    try
                    {
                        item2 = new ToolStripMenuItem(Trim(node2.Attributes["text"].Value));
                    }
                    catch
                    {
                        continue;
                    }
                    try
                    {
                        item2.Tag = noDis.Replace(Trim(node2.Attributes["class"].Value), "");
                    }
                    catch
                    {
                        item2.Tag = "";
                    }
                    if (HasClass(node2, "new-subject"))
                        item2.Click += new EventHandler(NewSubject);
                    if (HasClass(node2, "delete-empty-subjects"))
                        item2.Click += new EventHandler(DeleteEmptySubjects);
                    if (HasClass(node2, "new-assignment"))
                        item2.Click += new EventHandler(NewAssignment);
                    if (HasClass(node2, "delete-subject"))
                        item2.Click += new EventHandler(DeleteSubject);
                    if (item2.Tag.ToString() == "")
                        item2.Tag = null;
                    foreach (XmlNode node3 in node2.ChildNodes)
                    {
                        if (node3.Name != "item")
                            continue;
                        try
                        {
                            item3 = new ToolStripMenuItem(Trim(node3.Attributes["text"].Value));
                        }
                        catch
                        {
                            continue;
                        }
                        try
                        {
                            item3.Tag = noDis.Replace(Trim(node3.Attributes["class"].Value), "");
                        }
                        catch
                        {
                            item3.Tag = "";
                        }
                        if (HasClass(node3, "delete-assignment"))
                            item3.Click += new EventHandler(DeleteAssignment);
                        if (HasClass(node3, "edit-assignment"))
                            item3.Click += new EventHandler(EditAssignment);
                        if (HasClass(node3, "time-est"))
                        {
                            string est = item3.Text.ToLower();
                            try
                            {
                                int tm = Int32.Parse(est.Remove(est.IndexOf(' ')));
                                string units = est.Substring(est.IndexOf(' '));
                                if (!units.EndsWith("s"))
                                    units += "s";
                                if (units == "hours")
                                {
                                    tm *= 60;
                                    units = "minutes";
                                }
                            }
                            catch { }
                        }
                        if (HasClass(node3, "due-date"))
                        {
                            if (item3.Text.ToLower() == "due in two days")
                            {
                                if (LastVisit.AddDays(1).DayOfYear == DateTime.Now.DayOfYear)
                                {
                                    item3.Text = "Due Tomorrow";
                                    ++ShortTerm;
                                    --LongTerm;
#if !SUPPRESS_STARTUP_MSGBXS
                                    MessageBox.Show("The " + item1.Text + " assignment, \"" +
                                        item2.Text + ",\" is due tomorrow.", "Due Tomorrow",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif
                                }
#if !SUPPRESS_STARTUP_MSGBXS
                                else MessageBox.Show("The " + item1.Text + " assignment, \"" +
                                    item2.Text + ",\" is due in two days.", "Due In Two Days",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif
                            }
                            else if (Due.IsMatch(item3.Text))
                                try
                                {
                                    char[] sep = new char[] { '/', '-' };
                                    string year;
                                    try
                                    {
                                        year = item3.Text.Split(sep)[2].Split(' ')[0];
                                    }
                                    catch
                                    {
                                        year = DateTime.Now.Year.ToString();
                                    }
                                    DateTime due = new DateTime((new Regex(@"^due\s\d(\d?)(-|\/)" +
                                        @"\d(\d?)\2\d\d(\d?)(\d?)", RegexOptions.IgnoreCase))
                                        .IsMatch(item3.Text) ? Int32.Parse(year.Length < 4 ?
                                        DateTime.Now.Year.ToString().Remove(4 - year.Length) + year
                                        : (year.Length == 4 ? year : year.Remove(4))) : DateTime
                                        .Now.Year, Int32.Parse(item3.Text.Remove(item3.Text
                                        .IndexOfAny(sep)).Substring(4)), Int32.Parse(item3.Text
                                        .Split(sep)[1].Split(sep)[0]));
                                    if (DateTime.Now.AddDays(1).DayOfYear == due.DayOfYear)
                                    {
                                        item3.Text = "Due Tomorrow";
                                        --LongTerm;
                                        ++ShortTerm;
#if !SUPPRESS_STARTUP_MSGBXS
                                        MessageBox.Show("The " + item1.Text + " assignment, \"" +
                                            item2.Text + ",\" is due tomorrow.", "Due Tomorrow",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif
                                    }
                                    else if (DateTime.Now.AddDays(2).DayOfYear == due.DayOfYear)
                                    {
                                        item3.Text = "Due In Two Days";
#if !SUPPRESS_STARTUP_MSGBXS
                                        MessageBox.Show("The " + item1.Text + " assignment, \"" +
                                            item2.Text + ",\" is due in two days.",
                                            "Due In Two Days", MessageBoxButtons.OK, MessageBoxIcon
                                            .Information);
#endif
                                    }
                                    else if (DateTime.Now.DayOfYear == due.DayOfYear && DateTime
                                        .Now.Year == due.Year)
                                    {
                                        item3.Text = "Due Today";
#if !SUPPRESS_STARTUP_MSGBXS
                                        MessageBox.Show("The " + item1.Text + " assignment, \"" +
                                            item2.Text + ",\" was due today.", "Due Today",
                                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#endif
                                    }
                                    else if (DateTime.Now.Year > due.Year || DateTime.Now.DayOfYear
                                        > due.DayOfYear)
                                    {
                                        item3.Text = "Past Due Date";
#if !SUPPRESS_STARTUP_MSGBXS
                                        MessageBox.Show("The " + item1.Text + " assignment, \"" +
                                            item2.Text + ",\" is past the due date.",
                                            "Past Due Date", MessageBoxButtons.OK, MessageBoxIcon
                                            .Exclamation);
#endif
                                    }
                                }
                                catch { }
                            else if (item3.Text.ToLower() == "due tomorrow")
                            {
                                if (DateTime.Now.Year > LastVisit.Year || DateTime.Now.DayOfYear
                                    > LastVisit.DayOfYear)
                                {
                                    item3.Text = "Due Today";
#if !SUPPRESS_STARTUP_MSGBXS
                                    MessageBox.Show("The " + item1.Text + " assignment, \"" +
                                        item2.Text + ",\" was due today.", "Due Today",
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#endif
                                }
#if !SUPPRESS_STARTUP_MSGBXS
                                else MessageBox.Show("The " + item1.Text + " assignment, \"" +
                                    item2.Text + ",\" is due tomorrow.", "Due Tomorrow",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif
                            }
                            else if (item3.Text.ToLower() == "due today" && (DateTime.Now.Year >
                                LastVisit.Year || DateTime.Now.DayOfYear > LastVisit.DayOfYear))
                            {
                                item3.Text = "Past Due Date";
#if !SUPPRESS_STARTUP_MSGBXS
                                MessageBox.Show("The " + item1.Text + " assignment, \"" +
                                    item2.Text + ",\" is past the due date.", "Past Due Date",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#endif
                            }
#if !SUPPRESS_STARTUP_MSGBXS
                            else if (item3.Text.ToLower() == "past due date")
                                MessageBox.Show("The " + item1.Text + " assignment, \"" +
                                    item2.Text + ",\" is past the due date.", "Past Due Date",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#endif
                        }
                        if (item3.Tag.ToString() == "")
                            item3.Tag = null;
                        item3.Enabled = !HasClass(node3, "disabled");
                        item2.DropDownItems.Add(item3);
                    }
                    item2.Enabled = !HasClass(node2, "disabled");
                    item1.DropDownItems.Add(item2);
                }
                item1.Enabled = !HasClass(node1, "disabled");
                menu.Items.Add(item1);
            }
            Controls.Add(menu);
        }

        private int LongTerm = 0, ShortTerm = 0;

        private void NewSubject(object sender, EventArgs e)
        {
            new Subject(this);
            UpdateXml();
        }

        public void NewSubject(string reply)
        {
            foreach (ToolStripMenuItem item in menu.Items)
                if (item.Text.ToLower() == reply.ToLower())
                    return;
            ToolStripMenuItem subject = new ToolStripMenuItem(reply),
                assignment = new ToolStripMenuItem("Add an assignment"),
                del = new ToolStripMenuItem("Delete this subject");
            assignment.Click += new EventHandler(NewAssignment);
            assignment.Tag = "new-assignment";
            del.Click += new EventHandler(DeleteSubject);
            del.Tag = "delete-subject";
            subject.DropDownItems.Add(assignment);
            subject.DropDownItems.Add(del);
            menu.Items.Add(subject);
        }

        private void DeleteSubject(object sender, EventArgs e = null)
        {
            int i = 0;
            foreach (ToolStripMenuItem subject in menu.Items)
            {
                if (subject.DropDownItems.Count > 1)
                    if (subject.DropDownItems[1].Equals(sender))
                    {
                        menu.Items.RemoveAt(i);
                        break;
                    }
                ++i;
            }
        }

        private void DeleteEmptySubjects(object sender, EventArgs e = null)
        {
            int i = 0;
            foreach (ToolStripMenuItem subject in menu.Items)
            {
                if (subject.DropDownItems.Count == 2)
                    if (!subject.DropDownItems[1].Equals(sender))
                    {
                        menu.Items.RemoveAt(i);
                        DeleteEmptySubjects(sender);
                        break;
                    }
                ++i;
            }
        }

        private void NewAssignment(object sender, EventArgs e)
        {
            new Assignment(this, sender);
            UpdateXml();
        }

        public void NewAssignment(string reply, string dueDate, object sender, int pos = -1)
        {
            ToolStripMenuItem assignment = null, del = null, due = null, edit = null;
            string DueDate = Trim(dueDate);
            foreach (ToolStripMenuItem subject in menu.Items)
            {
                if (subject.DropDownItems[0].Equals(sender))
                {
                    subject.DropDownItems[1].Enabled = false;
                    assignment = new ToolStripMenuItem(Trim(reply));
                    if (DueDate != "")
                    {
                        if (DueDate.ToLower() == "tomorrow")
                            ++ShortTerm;
                        else ++LongTerm;
                        due = new ToolStripMenuItem("Due " + DueDate);
                        due.Enabled = false;
                        due.Tag = "due-date";
                        assignment.DropDownItems.Add(due);
                    }
                    edit = new ToolStripMenuItem("Edit this assignment");
                    edit.Click += new EventHandler(EditAssignment);
                    edit.Tag = "edit-assignment";
                    assignment.DropDownItems.Add(edit);
                    del = new ToolStripMenuItem("Delete this assignment");
                    del.Click += new EventHandler(DeleteAssignment);
                    del.Tag = "delete-assignment";
                    assignment.DropDownItems.Add(del);
                    if (pos < 0)
                        subject.DropDownItems.Add(assignment);
                    else subject.DropDownItems.Insert(pos, (ToolStripItem)assignment);
                    break;
                }
            }
        }

        private void EditAssignment(object sender, EventArgs e)
        {
            int i = 0;
            foreach (ToolStripMenuItem subject in menu.Items)
            {
                foreach (ToolStripMenuItem assignment in subject.DropDownItems)
                {
                    if (assignment.DropDownItems.Count > 1 && (assignment.DropDownItems[0]
                        .Equals(sender) || assignment.DropDownItems[1].Equals(sender)))
                    {
                        string due = assignment.DropDownItems[1].Equals(sender) ? (new Regex(
                            @"^due\s", RegexOptions.IgnoreCase)).Replace(assignment.DropDownItems
                            [0].Text, "") : "";
                        new Assignment(this, assignment.Text, due, subject, i);
                        UpdateXml();
                        return;
                    }
                    ++i;
                }
                i = 0;
            }
        }

        public void EditAssignment(Assignment edit, string due, ToolStripMenuItem subject, int i)
        {
            if (edit.Replies == null)
                return;
            if (due.ToLower() == "tomorrow")
                --ShortTerm;
            else if (due != "")
                --LongTerm;
            subject.DropDownItems.RemoveAt(i);
            NewAssignment(edit.Replies[0], edit.Replies[1], subject.DropDownItems
                [0], i);
        }

        private void DeleteAssignment(object sender, EventArgs e)
        {
            int i = 0;
            foreach (ToolStripMenuItem subject in menu.Items)
            {
                foreach (ToolStripMenuItem assignment in subject.DropDownItems)
                {
                    if (assignment.DropDownItems.Count == 2)
                        if (assignment.DropDownItems[1].Equals(sender))
                        {
                            subject.DropDownItems.RemoveAt(i);
                            if (subject.DropDownItems.Count == 2)
                                subject.DropDownItems[1].Enabled = true;
                            return;
                        }
                    if (assignment.DropDownItems.Count == 3)
                        if (assignment.DropDownItems[2].Equals(sender))
                        {
                            if (assignment.DropDownItems[0].Text.Substring(4).ToLower() ==
                                "tomorrow")
                                --ShortTerm;
                            else --LongTerm;
                            subject.DropDownItems.RemoveAt(i);
                            if (subject.DropDownItems.Count == 2)
                                subject.DropDownItems[1].Enabled = true;
                            return;
                        }
                    ++i;
                }
                i = 0;
            }
        }
    }
}
