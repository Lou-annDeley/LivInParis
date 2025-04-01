using System;
using System.Windows.Forms;



namespace Interface_LivInParis___DELPIERRE_DROUIN_DELEY
{
    public class MyForm : Form
    {
        public MyForm()
        {
            Button btn = new Button();
            btn.Text = "Cliquez-moi";
            btn.Location = new System.Drawing.Point(50, 50);
            btn.Click += (sender, e) => MessageBox.Show("Bonjour !");
            Controls.Add(btn);
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new MyForm());
        }
    }
}