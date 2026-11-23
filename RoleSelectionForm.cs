using System.Drawing;
using System.Windows.Forms;
using PawHaven.Models;

namespace PawHaven.UI;

public class RoleSelectionForm : Form
{
    public RoleSelectionForm()
    {
        Text = "PawHaven - Welcome";
        Size = new Size(900, 600);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = ColorTranslator.FromHtml("#FFF3D1");

        BuildUi();
    }

    private void BuildUi()
    {
        var brownCard = new Panel
        {
            Size = new Size(740, 480),
            Location = new Point(72, 40),
            BackColor = ColorTranslator.FromHtml("#4D4135")
        };

        var creamInner = new Panel
        {
            Size = new Size(520, 380),
            Location = new Point(110, 50),
            BackColor = ColorTranslator.FromHtml("#FFF3D1")
        };

        var lblTitle = new Label
        {
            Text = "PawHaven",
            Font = new Font("Segoe UI", 26, FontStyle.Bold | FontStyle.Italic),
            ForeColor = ColorTranslator.FromHtml("#4D4135"),
            Size = new Size(520, 55),
            Location = new Point(0, 25),
            TextAlign = ContentAlignment.MiddleCenter
        };

        var lblTagline = new Label
        {
            Text = "Where every paw finds a home",
            Font = new Font("Segoe UI", 11, FontStyle.Italic),
            ForeColor = ColorTranslator.FromHtml("#4D4135"),
            Size = new Size(520, 30),
            Location = new Point(0, 82),
            TextAlign = ContentAlignment.MiddleCenter
        };

        var lblWelcome = new Label
        {
            Text = "Welcome!",
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#4D4135"),
            Size = new Size(520, 35),
            Location = new Point(0, 125),
            TextAlign = ContentAlignment.MiddleCenter
        };

        var btnAdopter = MakeRoleButton("Adopter", 175);
        btnAdopter.Click += (_, _) => OpenAuth("Adopter");

        var btnAdmin = MakeRoleButton("Admin", 245);
        btnAdmin.Click += (_, _) => OpenAuth("Admin");

        creamInner.Controls.AddRange(new Control[] { lblTitle, lblTagline, lblWelcome, btnAdopter, btnAdmin });
        brownCard.Controls.Add(creamInner);
        Controls.Add(brownCard);
    }

    private static Button MakeRoleButton(string text, int top)
    {
        var btn = new Button
        {
            Text = text,
            Font = new Font("Segoe UI", 13, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#4D4135"),
            BackColor = ColorTranslator.FromHtml("#FCD12A"),
            FlatStyle = FlatStyle.Flat,
            Size = new Size(340, 50),
            Location = new Point(90, top),
            Cursor = Cursors.Hand
        };
        btn.FlatAppearance.BorderSize = 0;
        return btn;
    }

    private void OpenAuth(string role)
    {
        UserSession.SelectedRole = role;
        var auth = new AuthForm();
        auth.FormClosed += (_, _) => Show();
        auth.Show();
        Hide();
    }
}