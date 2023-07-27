using KAutoHelper;
using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);
        private const int WM_CHAR = 0x0102;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int VK_TAB = 0x09;
        private const int VK_RETURN = 0x0D;
        bool isrunning = false;
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            loadata();
            loadtoado();
        }
        void loadtoado()
        {
            if (File.Exists("toado.txt"))
            {

                var list_data = File.ReadAllLines("toado.txt");
                if (list_data.Length > 12)
                {

                    numinputphoneX.Value = int.Parse(list_data[0]);
                    numinputphoneY.Value= int.Parse(list_data[1]);
                    numnextsendX.Value = int.Parse(list_data[2]);
                    numnextsendY.Value = int.Parse(list_data[3]);
                    numdragcapchaX.Value = int.Parse(list_data[4]);
                    numdragcapchaY.Value = int.Parse(list_data[5]);
                    numdragToX.Value = int.Parse(list_data[6]);
               
                    numfirst_codeX.Value = int.Parse(list_data[7]);
                    numfirst_codeY.Value = int.Parse(list_data[8]);
                    num_next_to_mailX.Value= int.Parse(list_data[9]);
                    num_next_to_mailY.Value = int.Parse(list_data[10]);
                    numsubmitX.Value = int.Parse(list_data[11]);
                    numsubmitY.Value = int.Parse(list_data[12]);
                    numsonhaX.Value = int.Parse(list_data[13]);
                    numsonhaY.Value = int.Parse(list_data[14]);
















                }
            }
        }
        void loadata()
        {
            if (File.Exists("data.txt"))
            {

                var list_data = File.ReadAllLines("data.txt");
                if (list_data.Length > 5)
                {
                    tb_ref.Text = list_data[0];
                    textBox1.Text = list_data[1];
                    textBox2.Text = list_data[2];
                    tb_proxyapi.Text = list_data[3];
                    textBox3.Text = list_data[4];
                    numpricefrom.Value = int.Parse(list_data[5]);
                    numpriceto.Value = int.Parse(list_data[6]);
                    tb_emailapi.Text = list_data[7];
                    tbsavecmnd.Text = list_data[8];

                }
            }

        }
        public void DeleteLineInFile(string filePath, string inputString)
        {
            string[] lines = File.ReadAllLines(filePath);

            string tempFilePath = Path.GetTempFileName();

            using (StreamWriter writer = new StreamWriter(tempFilePath))
            {

                foreach (string line in lines)
                {

                    if (line == inputString)
                    {
                        continue;
                    }

                    writer.WriteLine(line);
                }
            }

            File.Delete(filePath);
            File.Move(tempFilePath, filePath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_ref.Text) || string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(tb_proxyapi.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin");
                return;
            }
            else
            {
                isrunning = true;
                autoAsync();

            }
        }


        private List<string> getuseragentstring()
        {
            string[] lines = { };
            if (!string.IsNullOrEmpty(textBox3.Text))
            {
                lines = File.ReadAllLines(textBox3.Text);

            }
            List<string> Result = new List<string>();
            Result.AddRange(lines);

            if (Result.Count > 0)
            {
                return Result;
            }
            return null;
        }
        static bool MoveAndRenameFolder(string currentFolderPath, string newFolderPath, string folderToMove, string newFolderName)
        {
            try
            {
                 string sourcePath = Path.Combine(currentFolderPath, folderToMove);

              
                string destinationPath = Path.Combine(newFolderPath, newFolderName);
 
                if (Directory.Exists(sourcePath))
                {
                    
                    if (!Directory.Exists(destinationPath))
                    {
                        Directory.CreateDirectory(destinationPath);
                    }


                    foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));
                    }

                    foreach (string filePath in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
                    {
                        string relativePath = filePath.Replace(sourcePath, "");
                        string destinationPathn = Path.Combine(destinationPath, relativePath.TrimStart(Path.DirectorySeparatorChar));
                        File.Move(filePath, destinationPathn);
                    }

                    Directory.Delete(sourcePath, true);



                    return true;
                }
                else
                {
                    
                    return false;
                }
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }
        public async void autoAsync()
        {
            while (isrunning)
            {
           
            string userAgent = "Mozilla/5.0 (Linux; Android 10; K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36";


            var useragentstring = getuseragentstring();
            if (useragentstring != null)
            {
                userAgent = useragentstring[new Random().Next(useragentstring.Count - 1)];

            }
            else
            {
                MessageBox.Show("lỗi khi đọc useragent");
                return;
            }


            var proxyhp = await ProxyHepler.getProxy(tb_proxyapi.Text);

            while (proxyhp == null)
            {
                proxyhp = await ProxyHepler.getProxy(tb_proxyapi.Text);
                Thread.Sleep(1000);
            }

            string proxyAddress = proxyhp.proxy.Trim();
            string url = tb_ref.Text;



        //  ADBHelper.ExecuteCMD("cd " + tb_chromepath.Text + " && chrome.exe --proxy-server='" + proxyAddress + "' "+url);
        //String user_agent_test = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_5_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.1.1 Mobile/15E148 Safari/604.1";
        //ProcessStartInfo startInfo = new ProcessStartInfo
        //{
        //    FileName = "chrome.exe",
        //    //   Arguments = " --proxy-server=116.101.48.216:5024 --new-window " + url,
        //    Arguments = " --start-maximized " + url,
        //    WindowStyle = ProcessWindowStyle.Maximized
        //};
        ////--incognito
        //Process chromeProcess = Process.Start(startInfo);

        //string pathchrome = @"C:\Program Files\Google\Chrome\Application";
        //ADBHelper.ExecuteCMD("cd /d " + pathchrome + " && chrome.exe --proxy-server=171.237.120.121:5034 --new-window " + url);

        GetpathCCCD:
            PathCMND path = null;
            int count_cmd = 0;
            while (path == null)
            {
                path = GetCMNDPath(textBox2.Text);
                Thread.Sleep(1000);
                count_cmd++;
                if (count_cmd == 10)
                {

                    MessageBox.Show("Hết CMND/CCCD");
                    return;
                }
            }
            String fontpath = "";
            String backpath = "";
            OcrIdCardResult info = null;
            var info_font = await FPTOCR.getresult(FPTOCR.ConvertImageToBase64(path.path1));
            if (info_font != null)
            {
                info = info_font;
                fontpath = path.path1;
                backpath = path.path2;
            }
            else
            if (info_font == null)
            {
                info_font = await FPTOCR.getresult(FPTOCR.ConvertImageToBase64(path.path2));
                if (info_font != null)
                {
                    info = info_font;

                    fontpath = path.path2;
                    backpath = path.path1;
                }
            }

            if (info_font == null)
            {
                if (Directory.Exists(path.origin_path))
                {
                    Directory.Delete(path.origin_path, true);

                }
                goto GetpathCCCD;
            }

        sendcode:

            Process.Start(new ProcessStartInfo
            {
                FileName = "chrome.exe",
                Arguments = $"--user-agent=\"{userAgent}\" --proxy-server=\"http={proxyAddress};https={proxyAddress}\" {url}"
            });
            var chromehandel = getchromemainhandel();



            while (chromehandel == IntPtr.Zero)
            {
                chromehandel = getchromemainhandel();
                Thread.Sleep(1000);
            }
            Thread.Sleep(6000);
            AutoControl.SendClickOnPosition(chromehandel,int.Parse(numinputphoneX.Value.ToString()), int.Parse(numinputphoneY.Value.ToString()));
            int count = 0;
            var number = await otphelper.getnumber(textBox1.Text);
            while (number == null)
            {
                number = await otphelper.getnumber(textBox1.Text);
                Thread.Sleep(1000);
            }
            Thread.Sleep(2000);
            AutoControl.SendClickOnPosition(chromehandel, int.Parse(numinputphoneX.Value.ToString()), int.Parse(numinputphoneY.Value.ToString()));
            if (chromehandel == IntPtr.Zero) AutoControl.MouseClick(int.Parse(numinputphoneX.Value.ToString()), int.Parse(numinputphoneY.Value.ToString()));
            Thread.Sleep(1000);
            //   Clipboard.SetText(number.Phone_number.ToString());

            sendkey(number.Phone_number, chromehandel);

            Thread.Sleep(3000);

            AutoControl.SendClickOnPosition(chromehandel, int.Parse(numnextsendX.Value.ToString()), int.Parse(numnextsendY.Value.ToString()));
            Thread.Sleep(3000);
            //Cursor.Position = new Point(630, 490);
            AutoControl.SendDragAndDropOnPosition(chromehandel, int.Parse(numdragcapchaX.Value.ToString()), int.Parse(numdragcapchaY.Value.ToString()), int.Parse(numdragToX.Value.ToString()), int.Parse(numdragcapchaY.Value.ToString()));
              Thread.Sleep(3000);

                if (chromehandel == IntPtr.Zero)
            {
                if (chromehandel == IntPtr.Zero) AutoControl.MouseClick(1100, 475);
            }
            Thread.Sleep(2000);
            var getcode = await otphelper.Getcode(number.Request_id, textBox1.Text);

            while (getcode.Code == null)
            {
                getcode = await otphelper.Getcode(number.Request_id, textBox1.Text);
                Thread.Sleep(1000);
                count++;
                if (count >= 50)
                {

                    SendMessage(chromehandel, 0x0010, IntPtr.Zero, IntPtr.Zero);
                    goto sendcode;
                }
            }
            Thread.Sleep(1000);

            AutoControl.MouseClick(int.Parse(numfirst_codeX.Value.ToString()), int.Parse(numfirst_codeY.Value.ToString()));
            if (chromehandel != IntPtr.Zero)
            {
                SendKeys.SendWait(getcode.Code);
                //  sendkey(getcode.Code, chromehandel);
            }
            else
                SendKeys.SendWait(getcode.Code);

            Thread.Sleep(5000);
            //Cursor.Position = new Point(630, 490);
            AutoControl.SendDragAndDropOnPosition(chromehandel, int.Parse(numdragcapchaX.Value.ToString()), int.Parse(numdragcapchaY.Value.ToString()), int.Parse(numdragToX.Value.ToString()), int.Parse(numdragcapchaY.Value.ToString()));
            Thread.Sleep(5000);

               AutoControl.SendDragAndDropOnPosition(chromehandel, int.Parse(numdragcapchaX.Value.ToString()), int.Parse(numdragcapchaY.Value.ToString()), int.Parse(numdragToX.Value.ToString()), int.Parse(numdragcapchaY.Value.ToString()));
               Thread.Sleep(3000);
            AutoControl.SendClickOnPosition(chromehandel, int.Parse(num_next_to_mailX.Value.ToString()), int.Parse(num_next_to_mailY.Value.ToString()));
            if (chromehandel == IntPtr.Zero)
            {
                if (chromehandel == IntPtr.Zero) AutoControl.MouseClick(int.Parse(num_next_to_mailX.Value.ToString()), int.Parse(num_next_to_mailY.Value.ToString()));
            }


            Thread.Sleep(5000);
              
            
           
            SendKeys.SendWait("{TAB}");
            Thread.Sleep(500);
            SendKeys.SendWait("{TAB}");
            Thread.Sleep(2500);
            String email_rand = ConvertnoVie(ConvertAccentedToNonAccented(info.name_card).Replace(" ", "")).ToLower() + new Random().Next(1000, 9999).ToString() + "@gmail.com";
            SendKeys.SendWait(email_rand);
            Thread.Sleep(500);
            SendKeys.SendWait("{TAB}");
            Thread.Sleep(500);
            SendKeys.SendWait("Son28121998!!");
            Thread.Sleep(500);
            SendKeys.SendWait("{TAB}");
            Thread.Sleep(500);
            SendKeys.SendWait("Son28121998!!");
            Thread.Sleep(500);
            SendKeys.SendWait("{TAB}");
            Thread.Sleep(500);
            SendKeys.SendWait("{ENTER}");
                
            Thread.Sleep(4500);
            SendKeys.SendWait("^+j");
            Thread.Sleep(1000);
            SendKeys.SendWait("^(l)");
            Thread.Sleep(1000);
                AutoControl.SendClickOnPosition(chromehandel, int.Parse(numsubmitX.Value.ToString()), int.Parse(numsubmitY.Value.ToString()));
                Thread.Sleep(1500);
            SendKeys.SendWait("^(l)");
            Thread.Sleep(1500);
                //String javasript = "var a=document.getElementsByTagName('label');    var myArray = [2, 6, 6, 5, 5,4,2,5,4,2];   var index=0;    myArray.forEach(function(element) {     var aa=index;      var b=index+element-1;      const range = b - aa + 1;   const randomNum = Math.floor(Math.random() * range) + aa; a[randomNum].click();      index=index+element;});";
                //Clipboard.SetText(javasript);
                //Thread.Sleep(2000);
                //SendKeys.SendWait("^(v)");

                //Thread.Sleep(4500);
                //SendKeys.SendWait("{ENTER}");
                //Thread.Sleep(1500);
                //String click_submit = "document.getElementsByTagName('button')[2].click();";
                //Clipboard.SetText(click_submit);
                //Thread.Sleep(500);
                //SendKeys.SendWait("^(v)");
                //Thread.Sleep(500);
                //SendKeys.SendWait("{ENTER}");
                //Thread.Sleep(5000);
                // SendKeys.SendWait("^(l)");
                String setaddurl = "https://sellercenter.lazada.vn/apps/todo/detail/address";
            Clipboard.SetText("window.location='" + setaddurl + "';");

            Thread.Sleep(3500);
            SendKeys.SendWait("^(v)");
            Thread.Sleep(500);
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(5000);
            SendKeys.SendWait("^(l)");
            Thread.Sleep(3500);
            SendKeys.SendWait("^(v)");
            Thread.Sleep(500);
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(5000);
            SendKeys.SendWait("^(l)");
            var adds_list = info_font.address.Split(',');
            var city = (adds_list.Length > 0) ? adds_list[adds_list.Length - 1] : "HÀ NỘI";
            string javscript_set_adds_setfuntion = "function getRandomInt(a, b) { const min = Math.ceil(a); const max = Math.floor(b);  return Math.floor(Math.random() * (max - min + 1)) + min};function getAddress() {    var em=document.getElementsByTagName('em');    var list_str=em[0].textContent.split('/');   var result='';   for(var i=list_str.length-1;i>=0;i--){        var c=(i==0)?'':',';        result+=list_str[i].trim()+c;   }  return result;} function getindexwithstring(li,str) {   for(var i=0;i<li.length;i++){   if(li[i].textContent.toLowerCase()==str.toLowerCase()) return i;    }    return 0;} function copyText(text) { const textarea = document.createElement('textarea');  textarea.value = text; document.body.appendChild(textarea); textarea.select();   \r\n    document.execCommand('copy');  document.body.removeChild(textarea);}";
            string excute_setadd = " var list=document.getElementsByTagName('input'); list[0].click(); var ul=document.getElementsByTagName('ul'); var ul3=ul[3].getElementsByTagName('li');ul3[getindexwithstring(ul3,'" + city.Trim() + "')].click();await new Promise((resolve) => setTimeout(resolve, 1000));var ul4=ul[4].getElementsByTagName('li');ul4[getRandomInt(0,ul4.length-1)].click();await new Promise((resolve) => setTimeout(resolve, 1000));var ul5=ul[5].getElementsByTagName('li');ul5[getRandomInt(0,ul5.length-1)].click();await new Promise((resolve) => setTimeout(resolve, 1000));copyText(getAddress()); ";

            string final_scrip_add = javscript_set_adds_setfuntion + excute_setadd;
            Clipboard.SetText(final_scrip_add);
            Thread.Sleep(500);
            SendKeys.SendWait("^(v)");
            Thread.Sleep(500);
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(4000);
            string add = new Random().Next(1, 999).ToString();




            Thread.Sleep(1000);
            AutoControl.SendClickOnPosition(chromehandel, int.Parse(numsonhaX.Value.ToString()), int.Parse(numsonhaY.Value.ToString()));
            Thread.Sleep(1000);
            SendKeys.SendWait(add);
            Thread.Sleep(2000);
               AutoControl.SendClickOnPosition(chromehandel, int.Parse(numsubmitX.Value.ToString()), int.Parse(numsubmitY.Value.ToString()));
                Thread.Sleep(1500);
            SendKeys.SendWait("^(l)");

            string click_submita = "document.getElementsByTagName('button')[0].click();";
            Clipboard.SetText(click_submita);
            Thread.Sleep(500);
            SendKeys.SendWait("^(v)");
            Thread.Sleep(500);
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(6000);
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            AutoControl.MouseClick(screenWidth - 143, 50);
            Thread.Sleep(1000);
            using (StreamWriter wr = new StreamWriter("account.txt", true))
            {
                try
                {
                    wr.WriteLine(number.Phone_number);
                    wr.Close();
                }
                catch
                {

                }


            }
            Thread.Sleep(5000);

            SendMessage(chromehandel, 0x0010, IntPtr.Zero, IntPtr.Zero);
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--user-agent=" + userAgent);
                try
                {
                    DeleteLineInFile(textBox3.Text, userAgent);
                }
                catch
                {

                }
                options.AddArgument("--start-maximized");
               
                string extensionPath = "addcookie.crx";
            options.AddExtension(extensionPath);
                //Proxy proxy = new Proxy();
                //proxy.Kind = ProxyKind.Manual;
                //proxy.HttpProxy = proxyAddress;
                //proxy.SslProxy = proxyAddress;


                //options.Proxy = proxy;



                IWebDriver driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://sellercenter.lazada.vn/");
            Thread.Sleep(3000);
            AutoControl.MouseClick(screenWidth - 125, 50);
            Thread.Sleep(1000);
            AutoControl.MouseClick(screenWidth - 280, 195);
            Thread.Sleep(2000);
            driver.Navigate().GoToUrl("https://sellercenter.lazada.vn/");
            Thread.Sleep(2000);
            AutoControl.MouseClick(screenWidth / 2, 300);
            Thread.Sleep(3000);

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            reupcmnd:
                var div_a = driver.FindElements(By.ClassName("bottom-button"));
            while (div_a.Count < 1)
            {
                div_a = driver.FindElements(By.ClassName("bottom-button"));
                Thread.Sleep(1000);
            }
            Thread.Sleep(1000);
            var atag = div_a[0].FindElements(By.TagName("a"));
            while (atag.Count < 1)
            {
                atag = div_a[0].FindElements(By.TagName("a"));
                Thread.Sleep(1000);
            }
            Thread.Sleep(1000);
            atag[0].Click();
            Thread.Sleep(3000);
            string currentWindowHandle = driver.CurrentWindowHandle;
            string[] windowHandles = driver.WindowHandles.ToArray();
            if (windowHandles.Length > 1)
            {
                string desiredWindowHandle = windowHandles[windowHandles.Length - 1];
                if (desiredWindowHandle != currentWindowHandle)
                {
                    driver.SwitchTo().Window(desiredWindowHandle);
                }
            }
                Thread.Sleep(1000);
                string currenturl = driver.Url;
                if (currenturl.Trim().IndexOf("account") > -1)
                {
                    var btn_edit = driver.FindElements(By.ClassName("next-btn-helper"));
                    while (btn_edit.Count < 1)
                    {
                        btn_edit = driver.FindElements(By.ClassName("next-btn-helper"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(1000);
                    btn_edit[0].Click();
                    Thread.Sleep(1000);
                    var getnumberagain = await otphelper.getagiannumber(textBox1.Text, number.Phone_number);
                    while (getnumberagain == null)
                    {

                        getnumberagain = await otphelper.getagiannumber(textBox1.Text, number.Phone_number);
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(1000);
                    var btns_send = driver.FindElements(By.ClassName("next-btn-helper"));
                    while (btns_send.Count < 1)
                    {
                        btns_send = driver.FindElements(By.ClassName("next-btn-helper"));
                        Thread.Sleep(1000);
                    }

                    foreach (var btns in btns_send)
                    {
                        if (btns.Text.Trim() == "Gửi")
                        {
                            btns.Click();
                            break;
                        }
                    }
                    Thread.Sleep(1000);
                    var getcode2 = await otphelper.Getcode(getnumberagain.Request_id, textBox1.Text);
                    while (getcode2.Code == null)
                    {
                        getcode2 = await otphelper.Getcode(getnumberagain.Request_id, textBox1.Text);
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(1000);
                    var ipss = driver.FindElements(By.TagName("input"));
                    while (ipss.Count < 1)
                    {
                        ipss = driver.FindElements(By.TagName("input"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(1000);
                    foreach (var ip in ipss)
                    {
                        try
                        {

                            string plt = ip.GetAttribute("placeholder");
                            if (!string.IsNullOrEmpty(plt))
                            {
                                if (plt.Trim() == "Nhập mã")
                                {
                                    ip.SendKeys(getcode2.Code);
                                    break;
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                    Thread.Sleep(1000);
                    var btns_conti = driver.FindElements(By.ClassName("next-btn-helper"));
                    while (btns_conti.Count < 1)
                    {
                        btns_conti = driver.FindElements(By.ClassName("next-btn-helper"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(1000);
                    foreach (var btn in btns_conti)
                    {
                        if (btn.Text.Trim() == "Tiếp tục")
                        {
                            btn.Click();
                            break;
                        }
                    }
                    Thread.Sleep(1000);
                    var getmail = await otphelper.getgmail(tb_emailapi.Text);

                    while (getmail == null)
                    {
                        getmail = getmail = await otphelper.getgmail(tb_emailapi.Text);
                        Thread.Sleep(3000);
                    }
                changemai:
                    var ipmails = driver.FindElements(By.TagName("input"));
                    while (ipmails.Count < 1)
                    {
                        ipmails = driver.FindElements(By.TagName("input"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(1000);
                    foreach (var itemipmail in ipmails)
                    {
                        try
                        {

                            string plt = itemipmail.GetAttribute("placeholder");
                            if (!string.IsNullOrEmpty(plt))
                            {
                                if (plt.Trim() == "Email")
                                {

                                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value = arguments[1];", itemipmail, "");
                                    Thread.Sleep(1000);
                                    itemipmail.SendKeys(getmail.data);
                                    break;
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                    Thread.Sleep(1000);

                    btns_send = driver.FindElements(By.ClassName("next-btn-helper"));
                    while (btns_send.Count < 1)
                    {
                        btns_send = driver.FindElements(By.ClassName("next-btn-helper"));
                        Thread.Sleep(1000);
                    }
                    btns_send[4].Click();
                    Thread.Sleep(1000);
                    int countmail = 0;
                    var mailcode = await otphelper.GetGmailcode(tb_emailapi.Text, getmail.data);
                    while (mailcode.otp == null)
                    {
                        mailcode = await otphelper.GetGmailcode(tb_emailapi.Text, getmail.data);
                        Thread.Sleep(1000);

                    }
                    Thread.Sleep(1000);
                    ipss = driver.FindElements(By.TagName("input"));
                    while (ipss.Count < 1)
                    {
                        ipss = driver.FindElements(By.TagName("input"));
                        Thread.Sleep(1000);
                        countmail++;
                        if (countmail >= 50)
                        {
                            goto changemai;
                        }
                    }

                    Thread.Sleep(1000);
                    foreach (var ip in ipss)
                    {
                        try
                        {

                            string plt = ip.GetAttribute("placeholder");
                            if (!string.IsNullOrEmpty(plt))
                            {
                                if (plt.Trim() == "Nhập mã")
                                {
                                    ip.SendKeys(mailcode.otp);
                                    break;
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                    Thread.Sleep(1000);
                    btns_send[6].Click();
                    Thread.Sleep(2000);
                    var a_home = driver.FindElements(By.TagName("a"));
                    while (a_home.Count < 1)
                    {
                        a_home = driver.FindElements(By.TagName("a"));
                    }
                    Thread.Sleep(1000);
                    foreach (var atagh in a_home)
                    {
                        if (atagh.Text.Trim() == "Trang Chủ")
                        {
                            atagh.Click();
                            break;
                        }
                    }
                    Thread.Sleep(3000);
                    goto reupcmnd;



                }
                var list_ip = driver.FindElements(By.TagName("input"));
            while (list_ip.Count < 6)
            {
                list_ip = driver.FindElements(By.TagName("input"));
                Thread.Sleep(1000);
            }
            Thread.Sleep(1000);

            js.ExecuteScript("arguments[0].style.display = 'block';", list_ip[5]);
            js.ExecuteScript("arguments[0].style.display = 'block';", list_ip[7]);
            Thread.Sleep(1000);
            list_ip[5].SendKeys(fontpath);
            Thread.Sleep(1200);
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            Thread.Sleep(1000);
            list_ip[7].SendKeys(backpath);
            Thread.Sleep(1000);
            var name_el = driver.FindElement(By.Id("idName"));
            while (name_el == null)
            {
                name_el = driver.FindElement(By.Id("idName"));
                Thread.Sleep(1000);
            }
            Thread.Sleep(1000);
            name_el.SendKeys(info.name_card);
            Thread.Sleep(1000);
            var id_el = driver.FindElement(By.Id("idNumber"));
            while (id_el == null)
            {
                id_el = driver.FindElement(By.Id("idNumber"));
                Thread.Sleep(1000);
            }
            Thread.Sleep(1000);
            id_el.SendKeys(info.id_card);
            Thread.Sleep(1000);
            var btn_submits = driver.FindElements(By.TagName("button"));
            while (btn_submits.Count < 5)
            {
                btn_submits = driver.FindElements(By.TagName("button"));
                Thread.Sleep(1000);
            }
            Thread.Sleep(1000);
            if (list_ip.Count < 15)
            {
                btn_submits[4].Click();
                Thread.Sleep(3000);
             //   driver.Navigate().GoToUrl("https://sellercenter.lazada.vn/apps/setting/bank");


            }
            else
            {
                js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                Thread.Sleep(1000);
                js.ExecuteScript("arguments[0].style.display = 'block';", list_ip[11]);
                Thread.Sleep(1000);
                list_ip[11].SendKeys(path.stkimgepath);
                var bankname_el = driver.FindElement(By.Id("bankAccountName"));
                while (bankname_el == null)
                {
                    bankname_el = driver.FindElement(By.Id("bankAccountName"));
                    Thread.Sleep(1000);

                }
                Thread.Sleep(1000);
                bankname_el.SendKeys(ConvertnoVie(ConvertAccentedToNonAccented(info.name_card)).ToUpper());
                var txtstk = File.ReadAllText(path.stktxtpath);
                var arr_stk = txtstk.Trim().Split('|');
                if (arr_stk.Length < 2)
                {
                    MessageBox.Show("có lỗi khi đọc stk");
                    return;

                }
                else
                {
                    var banknumber_el = driver.FindElement(By.Id("bankAccountNumber"));
                    while (banknumber_el == null)
                    {
                        banknumber_el = driver.FindElement(By.Id("bankAccountNumber"));
                        Thread.Sleep(1000);

                    }
                    Thread.Sleep(3000);
                    banknumber_el.SendKeys(arr_stk[1]);
                    Thread.Sleep(1000);
                    var combobox_namebank = driver.FindElements(By.ClassName("next-select-trigger-search"));
                    while (combobox_namebank.Count < 1)
                    {
                        combobox_namebank = driver.FindElements(By.ClassName("next-select-trigger-search"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(1000);
                    combobox_namebank[0].Click();
                    Thread.Sleep(1000);
                    var uls = driver.FindElements(By.ClassName("next-menu"));
                    while (uls.Count < 1)
                    {
                        uls = driver.FindElements(By.ClassName("next-menu"));
                        Thread.Sleep(1000);
                    }
                    var lis = uls[0].FindElements(By.TagName("li"));
                    while (lis.Count < 1)
                    {
                        lis = uls[0].FindElements(By.TagName("li"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(1000);
                    foreach (var li in lis)
                    {
                        if (li.Text.ToLower().IndexOf(arr_stk[0].Trim().ToLower()) > -1)
                        {
                            li.Click();
                            break;
                        }
                    }
                    Thread.Sleep(1000);
                    btn_submits = driver.FindElements(By.TagName("button"));
                    while (btn_submits.Count < 5)
                    {
                        btn_submits = driver.FindElements(By.TagName("button"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(1000);
                    btn_submits[6].Click();
                }

            }
            Thread.Sleep(1000);
            if (Directory.Exists(path.origin_path))
            {
               // Directory.Delete(path.origin_path, true);
                    try
                    {
                        MoveAndRenameFolder(textBox2.Text, tbsavecmnd.Text, path.origin_path, number.Phone_number);
                    }
                    catch
                    {

                    }

                }
            Thread.Sleep(1000);
            var list_product_info = new List<productinfo>();
            await rungetsp(list_product_info);

            Thread.Sleep(1000);
            int numsp = new Random().Next(11, 12);
            for (int pr = 0; pr < list_product_info.Count; pr++)
            {
                if ((pr) > numsp) break;
                int check = 0;
            reup:
                try
                {

                    if (check == 0)
                    {
                            redelete:
                        driver.Navigate().GoToUrl("https://sellercenter.lazada.vn/apps/mediacenter");

                        //var divpool = driver.FindElements(By.ClassName("mc-selectable"));
                        //while (divpool.Count < 2)
                        //{
                        //    divpool = driver.FindElements(By.ClassName("mc-selectable"));
                        //    Thread.Sleep(1000);
                        //}
                        Thread.Sleep(3000);
                        var list_img_aviable = driver.FindElements(By.ClassName("dada-image-table__item"));
                        Thread.Sleep(2000);
                        if (list_img_aviable.Count > 0)
                        {
                            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                            Thread.Sleep(1000);
                            foreach (var img in list_img_aviable)
                            {
                                try
                                {

                                    img.Click();
                                }
                                catch
                                {

                                }
                            }
                            Thread.Sleep(2000);
                                
                            var btns = driver.FindElements(By.ClassName("next-btn"));
                                if (btns.Count < 16) goto redelete;

                            Thread.Sleep(500);
                                btns[15].Click();
                                       Thread.Sleep(1500);
                            btns = driver.FindElements(By.ClassName("next-btn"));
                           if(btns.Count < 18) goto redelete;
                            Thread.Sleep(500);
                            btns[17].Click();

                        }


                        var btn_upload = driver.FindElements(By.ClassName("next-btn-helper"));
                        while (btn_upload.Count < 1)
                        {
                            btn_upload = driver.FindElements(By.ClassName("next-btn-helper"));
                            Thread.Sleep(1000);
                        }
                        Thread.Sleep(500);
                        btn_upload[0].Click();
                        Thread.Sleep(1500);

                        for (int i = 0; i < list_product_info[pr].linksimg.Count; i++)
                        {
                            if (i >= 6) break;

                            if (i >= 9) break;
                            else
                            {
                                var upimage = driver.FindElements(By.TagName("input"));
                                while (upimage.Count < 4)
                                {
                                    upimage = driver.FindElements(By.TagName("input"));
                                    Thread.Sleep(1000);
                                }
                                Thread.Sleep(500);
                                js.ExecuteScript("arguments[0].style.display = 'block';", upimage[3]);
                                Thread.Sleep(500);
                                var path_img = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp.jpg");
                                await DownloadImage(list_product_info[pr].linksimg[i], path_img);
                                upimage[3].SendKeys(path_img);
                                Thread.Sleep(100);
                                var btn_upcontinue = driver.FindElements(By.ClassName("material-center-btn"));

                                while (btn_upcontinue.Count < 2)
                                {
                                    btn_upcontinue = driver.FindElements(By.ClassName("material-center-btn"));
                                    Thread.Sleep(1000);
                                }
                                Thread.Sleep(100);
                                btn_upcontinue[1].Click();
                            }
                        }
                        Thread.Sleep(3000);
                            int reclick = 0;
                    reclcik:
                        var cmf_upload = driver.FindElements(By.ClassName("material-center-btn"));
                        while (cmf_upload.Count < 1)
                        {
                            cmf_upload = driver.FindElements(By.ClassName("material-center-btn"));
                            Thread.Sleep(1000);
                        }
                        Thread.Sleep(500);
                        try
                        {
                            cmf_upload[0].Click();
                        }
                        catch
                        {

                        }

                      var  divpool = driver.FindElements(By.ClassName("mc-selectable"));
                        while (divpool.Count < 2)
                        {
                            divpool = driver.FindElements(By.ClassName("mc-selectable"));
                            Thread.Sleep(1000);
                        }

                        Thread.Sleep(500);
                        list_img_aviable = divpool[1].FindElements(By.ClassName("dada-image-table__item"));
                        if (list_img_aviable.Count < 1)
                            {
                                if (reclick <= 3)
                                {
                                    goto reclcik;
                                }
                                else
                                {
                                    goto redelete;
                                }
                            }
                        Thread.Sleep(2500);
                        driver.Navigate().GoToUrl("https://sellercenter.lazada.vn/apps/product/publish");
                    }
                        int count_check_pl = 0;
                    Thread.Sleep(500);
                        while (driver.Url.IndexOf("publish") <0)
                        {
                            count_check_pl++;
                             Thread.Sleep(1000);
                            if(count_check_pl >= 20)
                            {
                                driver.Navigate().GoToUrl("https://sellercenter.lazada.vn/apps/product/publish");
                                Thread.Sleep(3000);
                            }
                        }
                    var btn_chooseimg = driver.FindElements(By.TagName("button"));
                    while (btn_chooseimg.Count < 2)
                    {
                        btn_chooseimg = driver.FindElements(By.TagName("button"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(500);
                    btn_chooseimg[1].Click();
                    Thread.Sleep(1000);
                    var list_item = driver.FindElements(By.ClassName("dada-image-table__item"));
                    while (list_item.Count < 1)
                    {
                        list_item = driver.FindElements(By.ClassName("dada-image-table__item"));
                        Thread.Sleep(1000);

                    }
                    Thread.Sleep(500);
                    foreach (var item in list_item)
                    {
                        item.Click();
                    }
                    Thread.Sleep(500);
                    var btn_send = driver.FindElements(By.TagName("button"));
                    while (btn_send.Count < 13)
                    {
                        btn_send = driver.FindElements(By.TagName("button"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(500);
                    btn_send[12].Click();
                    Thread.Sleep(5000);
                    try
                    {

                        var checkdialog = driver.FindElements(By.ClassName("next-dialog"));
                        if (checkdialog.Count > 0)
                        {
                            var btn_okdlg = checkdialog[0].FindElements(By.ClassName("next-btn-helper"));
                            while (btn_okdlg.Count < 2)
                            {
                                btn_okdlg = checkdialog[0].FindElements(By.ClassName("next-btn-helper"));
                                Thread.Sleep(1000);
                            }
                            Thread.Sleep(500);
                            btn_okdlg[1].Click();
                        }
                    }
                    catch
                    {

                    }
                    Thread.Sleep(500);
                    var inputs = driver.FindElements(By.TagName("input"));
                    while (inputs.Count < 1)
                    {
                        inputs = driver.FindElements(By.TagName("input"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(500);
                        int check_count = 0;
                        re_setname:
                        try
                        {
                            string namepr = list_product_info[pr].name_product;
                            if (string.IsNullOrEmpty(namepr)) namepr = "sản phần hottrend 2023 "+new Random().Next(100,999);
                            inputs[0].SendKeys(namepr);
                            check_count = 0;
                        }
                        catch
                        {
                            if (inputs.Count > 0)
                            {
                                foreach (var input in inputs)
                                {
                                    try
                                    {
                                        var strip = input.GetAttribute("placeholder");
                                        if (!string.IsNullOrEmpty(strip))
                                        {
                                            if (strip.Trim().IndexOf("A300 Máy Ảnh") > -1)
                                            {
                                                string namepr = list_product_info[pr].name_product;
                                                if (string.IsNullOrEmpty(namepr)) namepr = "sản phần hottrend 2023 " + new Random().Next(100, 999);
                                                input.SendKeys(namepr);
                                                check_count = 0;
                                                break;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        check_count++;
                                    }

                                }
                            }
                            else check_count++;
                          

                        }
                       if(check_count > 0)
                        {
                            if (check_count < 5) goto re_setname;
                            else continue;
                        }
                    Thread.Sleep(1000);
                    var radios = driver.FindElements(By.ClassName("next-radio-wrapper"));
                    while (radios.Count < 1)
                    {
                        radios = driver.FindElements(By.ClassName("next-radio-wrapper"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(500);
                    var item_raselect = radios[new Random().Next(0, radios.Count - 1)];
                    item_raselect.Click();
                        js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                        Thread.Sleep(1000);
                    var ths = driver.FindElements(By.ClassName("next-select-trigger-search"));
                    while (ths.Count < 2)
                    {
                        ths = driver.FindElements(By.ClassName("next-select-trigger-search"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(500);
                        js.ExecuteScript("arguments[0].scrollIntoView(true);", ths[1]);
                        ths[1].Click();
                    Thread.Sleep(500);
                    var kths = driver.FindElements(By.ClassName("next-btn-helper"));

                    while (kths.Count < 20)
                    {
                        kths = driver.FindElements(By.ClassName("next-btn-helper"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(1500);
                        foreach (var th in kths)
                        {
                           if(th.Text.Trim().IndexOf("Không Có") > -1)
                            {
                                js.ExecuteScript("arguments[0].scrollIntoView(true);", th);
                                Thread.Sleep(1000);
                                th.Click();
                                break;
                            }
                        }
                     
                    Thread.Sleep(2500);
                    var ips = driver.FindElements(By.TagName("input"));
                    while (ips.Count < 1)
                    {
                        ips = driver.FindElements(By.TagName("input"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(500);
                    foreach (var ip in ips)
                    {
                        try
                        {
                            var placetext = ip.GetAttribute("placeholder");
                            if (!string.IsNullOrEmpty(placetext) && placetext.Trim().IndexOf("vào ở") > -1)
                            {
                                var list_ca = item_raselect.Text.Trim().Split('>');
                                string value_ca = "công cụ";
                                if (list_ca.Length > 0)
                                {
                                    value_ca = list_ca[list_ca.Length - 1].Trim();
                                }
                                ip.SendKeys(value_ca);
                                break;
                            }

                        }
                        catch
                        {

                        }
                    }
                    Thread.Sleep(1000);



                    Thread.Sleep(1000);
                    js.ExecuteScript("window.scrollTo(0,500);");
                    var price = driver.FindElement(By.Name("price"));
                    while (price == null)
                    {
                        price = driver.FindElement(By.Name("price"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(500);
                        int from = int.Parse(numpricefrom.Value.ToString());
                        int to=int.Parse(numpriceto.Value.ToString());
                    price.SendKeys(new Random().Next(from, to).ToString());
                    Thread.Sleep(500);
                    var khnumb = driver.FindElements(By.TagName("input"));
                    while (khnumb.Count() < 1)
                    {
                        khnumb = driver.FindElements(By.TagName("input"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(1000);
                    foreach (var kh in khnumb)
                    {
                        try
                        {
                            var text = kh.GetAttribute("placeholder");
                            if (text != null)
                            {
                                if (text == "Kho hàng" || text.IndexOf("Kho hàng") > -1)
                                {
                                    kh.SendKeys(new Random().Next(15, 80).ToString());
                                    break;
                                }
                            }


                        }
                        catch
                        {

                        }
                    }

                    Thread.Sleep(1000);
                    var btn_aprice = driver.FindElements(By.TagName("button"));

                    while (btn_aprice.Count < 22)
                    {
                        btn_aprice = driver.FindElements(By.TagName("button"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(500);
                    foreach (var btn in btn_aprice)
                    {
                        if (btn.Text.Trim().IndexOf("Áp dụng") > -1)
                        {
                            js.ExecuteScript("arguments[0].scrollIntoView(true);", btn);
                            Thread.Sleep(2000);
                            Thread.Sleep(1000);
                            btn.Click();
                            break;
                        }
                    }

                        try
                        {
                            var textaria = driver.FindElements(By.TagName("textarea"));
                            while (textaria.Count < 1)
                            {
                                textaria = driver.FindElements(By.TagName("textarea"));
                                Thread.Sleep(1000);
                            }
                            Thread.Sleep(1000);
                            js.ExecuteScript("arguments[0].style.display = 'block';", textaria[0]);
                            Thread.Sleep(500);
                            textaria[0].SendKeys(list_product_info[pr].dec);
                           // Thread.Sleep(1000);
                           // var uploadimg = driver.FindElements(By.ClassName("sc-kMrHXl"));
                           // while (uploadimg.Count < 1)
                           // {
                           //     uploadimg = driver.FindElements(By.ClassName("sc-kMrHXl"));
                           //     Thread.Sleep(1000);
                           // }
                           // Thread.Sleep(1000);
                           // uploadimg[0].Click();
                           // Thread.Sleep(1000);
                           // var imgs = driver.FindElements(By.ClassName("dada-image-table__item"));
                           // while(imgs.Count < 1)
                           // {
                           //     imgs = driver.FindElements(By.ClassName("dada-image-table__item"));
                           //     Thread.Sleep(1000);
                           // }
                           // Thread.Sleep(1000);
                           
                           // int max = new Random().Next(1, 2);
                           // if (imgs.Count < max) max = 1;
                           //for(int i=0;i<max;i++)
                           // {
                           //     imgs[i].Click();

                           // }
                           // Thread.Sleep(500);
                           // var btnsm = driver.FindElements(By.ClassName("next-btn-helper"));
                           // while(btnsm.Count < 1)
                           // {
                           //     btnsm = driver.FindElements(By.ClassName("next-btn-helper"));
                           //     Thread.Sleep(1000);
                           // }
                           // Thread.Sleep(1000);
                           // foreach(var btn in btnsm)
                           // {

                           //     if (btn.Text.Trim()=="Gửi")
                           //     {
                           //         btn.Click();
                           //         break;
                           //     }
                           // }
                        }
                        catch
                        {

                        }

                      
                    Thread.Sleep(1500);
                    js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                    Thread.Sleep(500);

                    var ems = driver.FindElements(By.TagName("em"));
                    while (ems.Count < 1)
                    {
                        ems = driver.FindElements(By.TagName("em"));
                        Thread.Sleep(1000);
                    }
                    foreach (var e in ems)
                    {
                        if (e.Text.Trim().ToLower() == "kg")
                        {
                            e.Click();
                            Thread.Sleep(1000);
                            var items = driver.FindElements(By.ClassName("next-menu-item"));
                            while (items.Count < 2)
                            {
                                items = driver.FindElements(By.ClassName("next-menu-item"));
                                Thread.Sleep(1000);
                            }
                            items[1].Click();
                            break;
                        }
                    }
                    Thread.Sleep(1500);
                    var card_div = driver.FindElements(By.ClassName("next-card-body"));
                    IWebElement div_c = null;
                    while (card_div.Count < 5)
                    {
                        card_div = driver.FindElements(By.ClassName("next-card-body"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(1000);
                    foreach (var card in card_div)
                    {
                        if (card.Text.Trim().IndexOf("Vân chuyển và Bảo hành") > -1)
                        {
                            div_c = card;
                            break;
                        }
                    }
                    Thread.Sleep(1000);
                    if (div_c == null) div_c = card_div[5];

                    Thread.Sleep(500);

                    var input_e = div_c.FindElements(By.TagName("input"));
                    while (input_e.Count < 1)
                    {
                        input_e = div_c.FindElements(By.TagName("input"));
                        Thread.Sleep(1000);

                    }
                    Thread.Sleep(500);
                    input_e[0].SendKeys(new Random().Next(200, 500).ToString());
                    Thread.Sleep(1000);
                    input_e[2].SendKeys((new Random().Next(25, 35)).ToString());
                    Thread.Sleep(1000);
                    input_e[3].SendKeys((new Random().Next(25, 35)).ToString());
                    Thread.Sleep(1000);
                    input_e[4].SendKeys((new Random().Next(25, 35)).ToString());
                    Thread.Sleep(100);
                    js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                    Thread.Sleep(1000);
                    var list_div_bh = driver.FindElements(By.ClassName("next-formily-item"));
                    foreach (var bh in list_div_bh)
                    {
                        if (bh.Text.Trim().IndexOf("Loại bảo") > -1)
                        {
                            var ipph = bh.FindElements(By.TagName("input"));
                            while (ipph.Count < 1)
                            {
                                ipph = bh.FindElements(By.TagName("input"));
                            }
                            Thread.Sleep(2000);
                             ipph[0].Click();
                                Thread.Sleep(2000);
                                js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                                Thread.Sleep(2000);
                            var list_item_bh = driver.FindElements(By.ClassName("next-menu-item-text"));
                            while (list_item_bh.Count < 1)
                            {
                                list_item_bh = driver.FindElements(By.ClassName("next-menu-item-text"));
                                Thread.Sleep(1000);
                            }
                            reclick:
                            Thread.Sleep(1000);
                                int check_err = 0;
                                try
                                {
                                    list_item_bh[new Random().Next(0, list_item_bh.Count - 1)].Click();

                                }
                                catch
                                {
                                    check_err = 1;
                                    js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                                }
                                if(check_err==1)
                                {
                                    Thread.Sleep(1000);
                                    goto reclick;
                                }
                                break;
                        }
                    }
                    //input_e[35].Click();
                    //Thread.Sleep(1000);
                    //var list_item_bh = driver.FindElements(By.ClassName("next-menu-item-text"));
                    //while (list_item_bh.Count < 1)
                    //{
                    //    list_item_bh = driver.FindElements(By.ClassName("next-menu-item-text"));
                    //    Thread.Sleep(1000);
                    //}
                    //Thread.Sleep(1000);
                    //list_item_bh[new Random().Next(0, list_item_bh.Count - 1)].Click();
                    Thread.Sleep(3000);
                    js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

                    Thread.Sleep(2000);
                    var btnsubmits = driver.FindElements(By.TagName("button"));
                    while (btnsubmits.Count < 1)
                    {
                        btnsubmits = driver.FindElements(By.TagName("button"));
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(500);
                    IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;

                    foreach (var btnsm in btnsubmits)
                    {
                        if (btnsm.Text.Trim().IndexOf("Gửi") > -1)
                        {
                            jsExecutor.ExecuteScript("arguments[0].scrollIntoView(true);", btnsm);
                            Thread.Sleep(2000);
                                bool checkclick = false;
                                while (checkclick == false)
                                {
                                    try
                                    {

                                        btnsm.Click();
                                        checkclick = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        jsExecutor.ExecuteScript("arguments[0].scrollIntoView(true);", btnsm);
                                    }
                                }

                                break;
                        }
                    }

                    Thread.Sleep(5000);
                    check = 0;
                }
                catch
                {
                    check = 1;

                }
                if (check == 1)
                {
                    driver.Navigate().Refresh();
                    try
                    {
                        var al = driver.SwitchTo().Alert();
                        if (al != null)
                        {
                            al.Accept();
                        }
                    }
                    catch
                    {

                    }

                    Thread.Sleep(5000);
                    goto reup;
                }
            }

            Thread.Sleep (2000);
                driver.Quit();
        }
            
            //string openupcmnd = "document.getElementsByTagName('a')[3
            //].click();"; 
            //Clipboard.SetText(openupcmnd);
            //Thread.Sleep(500);
            //SendKeys.SendWait("^(v)");
            //Thread.Sleep(500);
            //SendKeys.SendWait("{ENTER}");
            //     SendMessage(chromehandel, 0x0010, IntPtr.Zero, IntPtr.Zero);

        }

        private string ConvertnoVie(string v)
        {
            var strletter = new char[] { 'ă', 'â', 'đ', 'ê', 'ô', 'ơ', 'ư' };
            var strletterlatin = new char[] { 'a', 'a', 'd', 'e', 'o', 'o', 'u' };
            var listletter = new List<letterspecical> { };
            v = v.ToLower();
            for (int i = 0; i < strletter.Length; i++)
            {
                try
                {
                    var item = new letterspecical();
                    item.originletter = strletter[i];
                    item.latinletter = strletterlatin[i];
                    listletter.Add(item);
                }
                catch
                {

                }
            }
            if (listletter.Count > 0)
            {


                var list_char = v.ToCharArray();
                string result = " ";
                for (int ch = 0; ch < list_char.Length; ch++)
                {
                    foreach (var itemletter in listletter)
                    {
                        if (list_char[ch] == itemletter.originletter)
                        {
                            list_char[ch] = itemletter.latinletter;
                            break;
                        }
                    }
                    result += list_char[ch];
                }
                if (!string.IsNullOrEmpty(result.Trim())) return result.Trim();
                return v;
            }
            return v;
        }


        public async Task DownloadImage(string imageUrl, string savePath)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile(imageUrl, savePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error downloading image: {ex.Message}");
                }
            }
        }
        public async Task rungetsp(List<productinfo> list_product_info)
        {
            redown:
            int check=0;
            try
            {

          
            var list_product =  getlistproduct();
            while (list_product == null)
            {
                list_product = getlistproduct();
                Thread.Sleep(1000);
            }
            foreach (var product in list_product)
            {
                if (list_product_info.Count <= 11)
                {

              
                Thread t = new Thread(() =>
                {
                    

                   
                        var products = getsigleproduct(product);
                    if (products != null)
                    {
                        list_product_info.Add(products);
                    }  
                });
                t.Start();
                }
                else
                {
                    break;
                }
            }
            while ( list_product_info.Count<10) ;
            }
            catch
            {
                check = 1;
            }
            if (check == 1) goto redown;
        }
        public productinfo getsigleproduct(string ads_product)
        {
            try
            {

          
            productinfo result = new productinfo();
            result.linksimg = new List<string>();
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
          //  options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");

            var driver = new ChromeDriver(chromeDriverService, options);
            driver.Navigate().GoToUrl(ads_product);
            var title = driver.FindElements(By.ClassName("pdp-mod-product-badge-title"));
            while (title.Count < 1)
            {
                title = driver.FindElements(By.ClassName("pdp-mod-product-badge-title"));
                Thread.Sleep(1000);
            }
            Thread.Sleep(100);

            result.name_product = title[0].Text.Trim();
            Thread.Sleep(200);
            var div_images = driver.FindElements(By.ClassName("next-slick-track"));
            while (div_images.Count < 1)
            {
                div_images = driver.FindElements(By.ClassName("next-slick-track"));
                Thread.Sleep(1000);

            }
            Thread.Sleep(100);
            var list_img = div_images[0].FindElements(By.TagName("img"));
            int count = 0;
            while (list_img.Count < 2)
            {
                count++;
                list_img = div_images[0].FindElements(By.TagName("img"));
                Thread.Sleep(1000);
                if (count == 10) return null;
            }
            Thread.Sleep(100);
            for (int i = 1; i < list_img.Count; i++)
            {
                try
                {

                    string src = list_img[i].GetAttribute("src").Trim().Replace("120x120", "720x720").Replace("80x80","720x720");
                    if (!string.IsNullOrEmpty(src))
                    {
                        result.linksimg.Add(src);

                    }

                }
                catch
                {

                }
            }
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;

                jsExecutor.ExecuteScript("window.scrollBy(0, 1000);");
                var mota = driver.FindElements(By.ClassName("html-content"));
                while(mota.Count <1)
                {
                    mota = driver.FindElements(By.ClassName("html-content"));
                    Thread.Sleep(1000);
                }
                Thread.Sleep(500);
                if (mota.Count < 2)
                {
                    string content = mota[0].Text;

                    result.dec = content;
                }
                else
                {
                    string content = mota[0].Text + "\n" + mota[1].Text;

                    result.dec = content;
                }
                driver.Quit();
            if (result.linksimg.Count > 1) return result;
            }
            catch
            {
               
                return null;
            }
        
            return null;
        }
        public List<string> getlistproduct()
        {
            List<string> list_link_product = new List<string>();
            try
            {

         
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
           
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
        
            options.AddArgument("--start-maximized");
            var driver = new ChromeDriver(chromeDriverService, options);
            driver.Navigate().GoToUrl("https://www.lazada.vn/#hp-just-for-you");
        addmoreproduct:
            var div_content = driver.FindElements(By.ClassName("card-jfy-wrapper"));
            while (div_content.Count < 1)
            {
                div_content = driver.FindElements(By.ClassName("card-jfy-wrapper"));
                Thread.Sleep(1000);
            }
            Thread.Sleep(1000);
            var list_a_tag = div_content[0].FindElements(By.TagName("a"));
            while (list_a_tag.Count < 5)
            {
                list_a_tag = div_content[0].FindElements(By.TagName("a"));
                Thread.Sleep(1000);
            }
            Thread.Sleep(1000);
            foreach (var item in list_a_tag)
            {
                try
                {
                    string href = item.GetAttribute("href");
                    if (!string.IsNullOrEmpty(href) && href.IndexOf("pages") < 0 && checkexist(list_link_product, href.Trim().ToLower()) == false)
                    {
                        list_link_product.Add(href);
                        if (list_link_product.Count >= 16)
                        {
                            driver.Quit();
                            return list_link_product;
                        }

                    }
                }
                catch
                {

                }
            }

            if (list_link_product.Count < 12)
            {
                driver.Navigate().Refresh();
                Thread.Sleep(1500);
                goto addmoreproduct;
            }
            driver.Quit();
            }
            catch
            {
                return null;

            }
            return list_link_product;
        }
        public bool checkexist(List<string> list, string search)
        {
            foreach (var item in list)
            {
                int start = search.IndexOf("products/");
                string substring = search.Substring(start, 30);
                if (item.IndexOf(substring) > -1) { return true; }

            }
            return false;
        }
        public static string ConvertAccentedToNonAccented(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            string output = input.Normalize(NormalizationForm.FormD);
            output = new string(output.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray());

            return output;
        }
        void sendkey(string text, IntPtr chromehandel)
        {
            Task.Run(() =>
            {

                Thread.Sleep(2000);
                var listkey = text.ToCharArray();
                foreach (char c in listkey)
                {
                    SendMessage(chromehandel, WM_CHAR, (IntPtr)c, IntPtr.Zero);
                }




            });
        }
        private IntPtr getchromemainhandel()
        {
            Process[] procsChrome = Process.GetProcessesByName("chrome");
            foreach (Process proc in procsChrome)
            {

                if (proc.MainWindowHandle == IntPtr.Zero)
                {
                    continue;
                }
                else
                {
                    if (proc.MainWindowTitle.IndexOf("Seller") > -1)
                    {

                        return proc.MainWindowHandle;
                    }



                }

            }
            return IntPtr.Zero;
        }
        private PathCMND GetCMNDPath(String path)
        {
            string[] imageExtensions = { "*.jpg", "*.jpeg", "*.png", "*.gif", "*.bmp" };
            string[] txtextention = { "*.txt" };
            List<string> imagePaths = new List<string>();
            string[] allFordel = { };
            if (!string.IsNullOrEmpty(path))
            {
                allFordel = Directory.GetDirectories(path);
                if (allFordel.Length < 1) return null;
                foreach (string extension in imageExtensions)
                {
                    string[] files = Directory.GetFiles(allFordel[0], extension, SearchOption.TopDirectoryOnly);
                    imagePaths.AddRange(files);
                }

            }
            if (imagePaths.Count < 2)
            {
                try
                {

                    if (Directory.Exists(allFordel[0]))
                    {
                        Directory.Delete(allFordel[0], true);

                    }
                }
                catch
                {

                }
                return null;
            }
            var result = new PathCMND();
            if (imagePaths.Count >= 2)
            {




                result.path1 = imagePaths[0];
                result.path2 = imagePaths[1];

                result.origin_path = allFordel[0];
                string[] folders = Directory.GetDirectories(result.origin_path);
                if (folders.Length < 1) return null;
                List<string> imagestklist = new List<string>();
                foreach (string extension in imageExtensions)
                {
                    string[] files = Directory.GetFiles(folders[0], extension, SearchOption.TopDirectoryOnly);
                    imagestklist.AddRange(files);
                }
                if (imagestklist.Count < 1) return null;

                var txtfile = Directory.GetFiles(folders[0], "*.txt", SearchOption.TopDirectoryOnly);
                if (txtfile.Length < 1) return null;
                result.stkimgepath = imagestklist[0];
                result.stktxtpath = txtfile[0];


            }
            else return null;
            return result;

        }
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog select_foder = new FolderBrowserDialog();
            if (select_foder.ShowDialog() == DialogResult.OK)
            {
                var select_path = select_foder.SelectedPath;
                textBox2.Text = select_path;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("vui lòng điền đủ thông tin");
                return;
            }
            using (StreamWriter wr = new StreamWriter("data.txt"))
            {
                wr.WriteLine(tb_ref.Text);   
                wr.WriteLine(textBox1.Text);
                wr.WriteLine(textBox2.Text);

                wr.WriteLine(tb_proxyapi.Text);
                wr.WriteLine(textBox3.Text);
                wr.WriteLine(numpricefrom.Value.ToString());
                wr.WriteLine(numpriceto.Value.ToString());
                wr.WriteLine(tb_emailapi.Text);
                wr.WriteLine(tbsavecmnd.Text);
                wr.Close();
                MessageBox.Show("Lưu thành công");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog select_foder = new OpenFileDialog();
          
            if (select_foder.ShowDialog() == DialogResult.OK)
            {
                var select_path = select_foder.FileName;
                textBox3.Text = select_path;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!isrunning)
            {
                MessageBox.Show("chương trình chưa chay!");
            }
            else
            {
                isrunning = false;
                MessageBox.Show("Chương trình sẽ dừng sau lượt này");
            }
        }

        private void btn_save_position_Click(object sender, EventArgs e)
        {
            using(StreamWriter wr= new StreamWriter("toado.txt"))
            {
                try
                {
                    wr.WriteLine(numinputphoneX.Value.ToString());
                    wr.WriteLine(numinputphoneY.Value.ToString());
                    wr.WriteLine(numnextsendX.Value.ToString());
                    wr.WriteLine(numnextsendY.Value.ToString());
                    wr.WriteLine(numdragcapchaX.Value.ToString());
                    wr.WriteLine(numdragcapchaY.Value.ToString());
                    wr.WriteLine(numdragToX.Value.ToString());
                    wr.WriteLine(numfirst_codeX.Value.ToString());
                    wr.WriteLine(numfirst_codeY.Value.ToString());
                    wr.WriteLine(num_next_to_mailX.Value.ToString());
                    wr.WriteLine(num_next_to_mailY.Value.ToString());
                    wr.WriteLine(numsubmitX.Value.ToString());
                    wr.WriteLine(numsubmitY.Value.ToString());
                    wr.WriteLine(numsonhaX.Value.ToString());
                    wr.WriteLine(numsonhaY.Value.ToString());
                    wr.Close();
                    MessageBox.Show("luu thanh cong");
                }
                catch {

                    MessageBox.Show("luu khong thanh cong");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog select_foder = new FolderBrowserDialog();
            if (select_foder.ShowDialog() == DialogResult.OK)
            {
                var select_path = select_foder.SelectedPath;
                tbsavecmnd.Text = select_path;
            }
        }
    }
}
