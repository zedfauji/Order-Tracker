using System.Drawing;
using System.Windows.Forms;
namespace RestockMateApp.Forms
{
    partial class Form1
    {
        private void InitializeFormProperties()
        {
            this.Text = "RestockMate";
            this.ClientSize = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Optional: global styling or font overrides
            // this.Font = new Font("Segoe UI", 9F);
            // this.BackColor = Color.WhiteSmoke;
        }
    }
}