using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JWTClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private string _token = string.Empty;
        private const string _scheme = JwtBearerDefaults.AuthenticationScheme;// "Bearer";

        /// <summary>
        /// 模拟登陆
        /// </summary>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            //var tokenString = GenerateJSONWebToken();
            using var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
            //var param = JsonConvert.SerializeObject(new { user = "admin", password = "admin" });
            //var content = new StringContent(param, Encoding.UTF8, "application/json");  // 序列化类
            var param = new Dictionary<string, string>
            {
                { "user", "admin" },
                { "password", "admin" },
            };
            var content = new FormUrlEncodedContent(param);
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded");
            using var response = await httpClient.PostAsync("http://localhost:5000/api/Login/Login", content);
            if (response.IsSuccessStatusCode)
            {
                string token = await response.Content.ReadAsStringAsync();
                this._token = token;
                lb.Items.Add($"login success token:{token}");
            }
        }

        private async void btnGetList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var httpClient = new HttpClient();
                // 请求其它数据需设置Authorization
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_scheme, this._token);
                var response = await httpClient.GetAsync("http://localhost:5000/api/Login/GetList");
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    lb.Items.Add("GetListResult-Unauthorized");
                }

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    lb.Items.Add($"GetListResult-Authorized:{result}");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
