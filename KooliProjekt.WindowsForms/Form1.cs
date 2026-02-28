using System.Collections;
using System.Net.Http.Json;
using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            LoadData();
            base.OnLoad(e);
        }

        private async Task LoadData()
        {
            var url = "http://localhost:5086/api/categories/list";
            url += "?pageNumber=1&pageSize=10";

            using var client = new HttpClient();
            var response = await client.GetFromJsonAsync<OperationResult<PagedResult<Category>>>(url);

            dataGridView1.DataSource = response.Value.Results;

        }
    }
}
