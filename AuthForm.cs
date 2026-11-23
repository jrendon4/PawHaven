using System.Drawing;
using System.Windows.Forms;
using PawHaven.Models;
using PawHaven.Services;

namespace PawHaven.UI;

public class AuthForm : Form
{
    private readonly AuthRepository _authRepo;
    private readonly Panel _loginPanel = new();
    private readonly Panel _signUpPanel = new();

    public AuthForm()
    {
        string dataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PawHaven");
        _authRepo = new AuthRepository(Path.Combine(dataDir, "users.json"));

        Text = $"PawHaven - {UserSession.SelectedRole}";
        Size = new Size(900, 620);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = ColorTranslator.FromHtml("#FFF3D1");

        BuildLoginPanel();
        BuildSignUpPanel();

        Controls.Add(_loginPanel);
        Controls.Add(_signUpPanel);
        ShowLogin();
    }

    private void BuildLoginPanel()
    {
        _loginPanel.Size = new Size(440, 500);
        _loginPanel.Location = new Point(220, 40);
        _loginPanel.BackColor = ColorTranslator.FromHtml("#4D4135");

        var inner = new Panel { Size = new Size(410, 470), Location = new Point(15, 15), BackColor = ColorTranslator.FromHtml("#FFF3D1") };
        var header = new Panel { Size = new Size(410, 80), Location = new Point(0, 0), BackColor = ColorTranslator.FromHtml("#FCD12A") };

        var lblBrand = new Label
        {
            Text = "PawHaven",
            Font = new Font("Segoe UI", 18, FontStyle.Bold | FontStyle.Italic),
            ForeColor = ColorTranslator.FromHtml("#4D4135"),
            Location = new Point(20, 12),
            AutoSize = true
        };
        var lblSub = new Label
        {
            Text = $"Where every paw finds a home • {UserSession.SelectedRole} Login",
            Font = new Font("Segoe UI", 9, FontStyle.Italic),
            ForeColor = ColorTranslator.FromHtml("#4D4135"),
            Location = new Point(22, 48),
            AutoSize = true
        };
        header.Controls.AddRange(new Control[] { lblBrand, lblSub });

        var lblEmail = new Label { Text = "Email Address", Location = new Point(40, 115), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#4D4135") };
        var txtEmail = new TextBox { Location = new Point(40, 140), Size = new Size(330, 30), Font = new Font("Segoe UI", 11) };

        var lblPass = new Label { Text = "Password", Location = new Point(40, 190), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#4D4135") };
        var txtPass = new TextBox { Location = new Point(40, 215), Size = new Size(330, 30), Font = new Font("Segoe UI", 11), UseSystemPasswordChar = true };

        var btnLogin = new Button
        {
            Text = "Login",
            Location = new Point(115, 280),
            Size = new Size(180, 42),
            BackColor = ColorTranslator.FromHtml("#FCD12A"),
            ForeColor = ColorTranslator.FromHtml("#4D4135"),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnLogin.FlatAppearance.BorderSize = 0;

        btnLogin.Click += (_, _) =>
        {
            var user = _authRepo.Login(txtEmail.Text, txtPass.Text, UserSession.SelectedRole, out string err);
            if (user == null)
            {
                MessageBox.Show(err, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UserSession.CurrentUser = user;
            var main = new MainForm();
            main.FormClosed += (_, _) => Close();
            main.Show();
            Hide();
        };

        var lnkSignUp = new LinkLabel
        {
            Text = "No Account? Sign Up",
            Location = new Point(140, 340),
            AutoSize = true,
            LinkColor = ColorTranslator.FromHtml("#4D4135"),
            Visible = UserSession.SelectedRole == "Adopter"
        };
        lnkSignUp.Click += (_, _) => ShowSignUp();

        var btnBack = new Button
        {
            Text = "← Back",
            Location = new Point(20, 420),
            Size = new Size(80, 30),
            FlatStyle = FlatStyle.Flat,
            ForeColor = ColorTranslator.FromHtml("#4D4135")
        };
        btnBack.FlatAppearance.BorderSize = 0;
        btnBack.Click += (_, _) => Close();

        inner.Controls.AddRange(new Control[] { header, lblEmail, txtEmail, lblPass, txtPass, btnLogin, lnkSignUp, btnBack });
        _loginPanel.Controls.Add(inner);
    }

    private void BuildSignUpPanel()
    {
        _signUpPanel.Size = new Size(440, 530);
        _signUpPanel.Location = new Point(220, 25);
        _signUpPanel.BackColor = ColorTranslator.FromHtml("#4D4135");

        var inner = new Panel { Size = new Size(410, 500), Location = new Point(15, 15), BackColor = ColorTranslator.FromHtml("#FFF3D1") };
        var header = new Panel { Size = new Size(410, 65), Location = new Point(0, 0), BackColor = ColorTranslator.FromHtml("#FCD12A") };

        var lblBrand = new Label
        {
            Text = "PawHaven • Create Account",
            Font = new Font("Segoe UI", 14, FontStyle.Bold | FontStyle.Italic),
            ForeColor = ColorTranslator.FromHtml("#4D4135"),
            Location = new Point(20, 18),
            AutoSize = true
        };
        header.Controls.Add(lblBrand);

        var txtLast = AddField(inner, "Last Name", 80);
        var txtFirst = AddField(inner, "First Name", 140);

        var lblBday = new Label { Text = "Birthday", Location = new Point(40, 200), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#4D4135") };
        var dtpBday = new DateTimePicker
        {
            Location = new Point(40, 220),
            Size = new Size(160, 28),
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "MM/dd/yyyy",
            MaxDate = DateTime.Today
        };

        var lblGender = new Label { Text = "Gender", Location = new Point(215, 200), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#4D4135") };
        var cmbGender = new ComboBox { Location = new Point(215, 220), Size = new Size(155, 28), DropDownStyle = ComboBoxStyle.DropDownList };
        cmbGender.Items.AddRange(new object[] { "Male", "Female", "Prefer not to say" });

        var txtEmail = AddField(inner, "Email", 260);
        var txtPass = AddField(inner, "Password", 320, isPassword: true);

        var btnSignUp = new Button
        {
            Text = "Sign Up",
            Location = new Point(115, 395),
            Size = new Size(180, 40),
            BackColor = ColorTranslator.FromHtml("#FCD12A"),
            ForeColor = ColorTranslator.FromHtml("#4D4135"),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnSignUp.FlatAppearance.BorderSize = 0;

        btnSignUp.Click += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(txtLast.Text) ||
                string.IsNullOrWhiteSpace(txtFirst.Text) ||
                cmbGender.SelectedItem == null ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("Please fill out all fields.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!txtEmail.Text.Contains('@') || !txtEmail.Text.Contains('.'))
            {
                MessageBox.Show("Please enter a valid email address.", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPass.Text.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.", "Weak Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newUser = new User
            {
                LastName = txtLast.Text.Trim(),
                FirstName = txtFirst.Text.Trim(),
                Birthday = dtpBday.Value.ToString("MM/dd/yyyy"),
                Gender = cmbGender.SelectedItem.ToString()!,
                Email = txtEmail.Text.Trim(),
                Password = txtPass.Text
            };

            if (_authRepo.RegisterAdopter(newUser, out string err))
            {
                MessageBox.Show("Account created! Please log in.", "Welcome to PawHaven", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowLogin();
            }
            else
            {
                MessageBox.Show(err, "Sign Up Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        var lnkBackLogin = new LinkLabel
        {
            Text = "Already have an account? Login",
            Location = new Point(118, 450),
            AutoSize = true,
            LinkColor = ColorTranslator.FromHtml("#4D4135")
        };
        lnkBackLogin.Click += (_, _) => ShowLogin();

        inner.Controls.AddRange(new Control[] { header, lblBday, dtpBday, lblGender, cmbGender, btnSignUp, lnkBackLogin });
        _signUpPanel.Controls.Add(inner);
    }

    private static TextBox AddField(Panel parent, string label, int top, bool isPassword = false)
    {
        var lbl = new Label { Text = label, Location = new Point(40, top), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#4D4135") };
        var txt = new TextBox { Location = new Point(40, top + 20), Size = new Size(330, 28), UseSystemPasswordChar = isPassword };
        parent.Controls.Add(lbl);
        parent.Controls.Add(txt);
        return txt;
    }

    private void ShowLogin() { _loginPanel.Visible = true; _signUpPanel.Visible = false; }
    private void ShowSignUp() { _loginPanel.Visible = false; _signUpPanel.Visible = true; }
}