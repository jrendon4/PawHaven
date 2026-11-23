using System.Drawing;
using System.Windows.Forms;
using PawHaven.Models;
using PawHaven.Services;

namespace PawHaven.UI;

public class MainForm : Form
{
    private readonly AnimalRepository _repo;

    private readonly TextBox txtSearch = new() { PlaceholderText = "Search name, species, breed...", Width = 240 };
    private readonly ComboBox cboFilter = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130 };
    private readonly DataGridView grid = new();
    private readonly Label lblCount = new() { AutoSize = true, Padding = new Padding(0, 6, 0, 0) };

    public MainForm()
    {
        string role = UserSession.CurrentUser?.Role ?? "Guest";
        string firstName = UserSession.CurrentUser?.FirstName ?? "User";

        Text = $"PawHaven - Animal Shelter Management System ({role}: {firstName})";
        Size = new Size(1000, 600);
        StartPosition = FormStartPosition.CenterScreen;

        string dataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PawHaven");
        _repo = new AnimalRepository(Path.Combine(dataDir, "animals.json"));

        BuildUi();
        RefreshGrid();
    }

    private void BuildUi()
    {
        bool isAdmin = UserSession.CurrentUser?.Role == "Admin";

        var btnAdd = new Button { Text = "Add", Size = new Size(90, 30), Visible = isAdmin };
        var btnEdit = new Button { Text = "Edit", Size = new Size(90, 30), Visible = isAdmin };
        var btnDelete = new Button { Text = "Delete", Size = new Size(90, 30), Visible = isAdmin };
        var btnLogout = new Button { Text = "Logout", Size = new Size(90, 30) };

        cboFilter.Items.Add("All");
        foreach (var s in Enum.GetValues<AnimalStatus>()) cboFilter.Items.Add(s);
        cboFilter.SelectedIndex = 0;

        var toolbar = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, Padding = new Padding(8) };
        toolbar.Controls.AddRange(new Control[] { txtSearch, cboFilter, btnAdd, btnEdit, btnDelete, lblCount, btnLogout });

        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.MultiSelect = false;
        grid.RowHeadersVisible = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.Controls.Add(toolbar, 0, 0);
        layout.Controls.Add(grid, 0, 1);
        Controls.Add(layout);

        txtSearch.TextChanged += (_, _) => RefreshGrid();
        cboFilter.SelectedIndexChanged += (_, _) => RefreshGrid();
        btnAdd.Click += (_, _) => AddAnimal();
        btnEdit.Click += (_, _) => EditAnimal();
        btnDelete.Click += (_, _) => DeleteAnimal();
        btnLogout.Click += (_, _) =>
        {
            UserSession.Clear();
            Close();
        };

        // Only allow double-click editing if logged in as Admin
        grid.CellDoubleClick += (_, e) =>
        {
            if (isAdmin && e.RowIndex >= 0) EditAnimal();
        };
    }

    private void RefreshGrid()
    {
        AnimalStatus? status = cboFilter.SelectedIndex > 0
            ? (AnimalStatus)cboFilter.SelectedItem!
            : null;

        var animals = _repo.Search(txtSearch.Text, status);

        grid.DataSource = animals.Select(a => new
        {
            a.AnimalId,
            a.Name,
            Type = a.GetAnimalType(),
            a.Breed,
            a.Age,
            a.Gender,
            a.Status,
            Details = a.DisplayInfo()
        }).ToList();

        lblCount.Text = $"{animals.Count} animal(s)";
    }

    private Animal? SelectedAnimal()
    {
        string? id = grid.CurrentRow?.Cells["AnimalId"].Value?.ToString();
        return id == null ? null : _repo.GetById(id);
    }

    private void AddAnimal()
    {
        using var form = new AnimalForm();
        if (form.ShowDialog(this) == DialogResult.OK && form.Result != null)
        {
            _repo.Add(form.Result);
            RefreshGrid();
        }
    }

    private void EditAnimal()
    {
        var selected = SelectedAnimal();
        if (selected == null)
        {
            MessageBox.Show("Select an animal first.", "PawHaven");
            return;
        }

        using var form = new AnimalForm(selected);
        if (form.ShowDialog(this) == DialogResult.OK && form.Result != null)
        {
            _repo.Update(form.Result);
            RefreshGrid();
        }
    }

    private void DeleteAnimal()
    {
        var selected = SelectedAnimal();
        if (selected == null)
        {
            MessageBox.Show("Select an animal first.", "PawHaven");
            return;
        }

        var answer = MessageBox.Show($"Delete {selected.Name} ({selected.AnimalId})?",
            "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (answer == DialogResult.Yes)
        {
            _repo.Delete(selected.AnimalId);
            RefreshGrid();
        }
    }
}