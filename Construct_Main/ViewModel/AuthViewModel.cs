using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BLL;
using MaterialDesignThemes.Wpf;

namespace Construct_Main.ViewModel
{
    public class AuthViewModel : INotifyPropertyChanged
    {
        private IAutorizationService authService;
        private MainViewModel mainWindow;

        #region NotifyPropertyChanged

        private string _mainText = "";
        public string MainText
        {
            get { return _mainText; }
            set
            {
                _mainText = value;
                NotifyPropertyChanged("MainText");
            }
        }

        private Visibility _isauth = Visibility.Visible;
        public Visibility IsAuth
        {
            get { return _isauth; }
            set
            {
                _isauth = value;
                NotifyPropertyChanged("IsAuth");
            }
        }

        private Visibility _isin = Visibility.Visible;
        public Visibility IsIn
        {
            get { return _isin; }
            set
            {
                _isin = value;
                NotifyPropertyChanged("IsIn");
            }
        }
        private Visibility _isreg = Visibility.Visible;
        public Visibility IsReg
        {
            get { return _isreg; }
            set
            {
                _isreg = value;
                NotifyPropertyChanged("IsReg");
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Command
        private RelayCommand auth;
        public RelayCommand AuthCommand
        {
            get
            {
                return auth ?? (auth = new RelayCommand(obj =>
                {
                    var values = (object[])obj;
                    string login = (string)values[0];
                    PasswordBox password = (PasswordBox)values[1];
                    Authorization(login, password.Password);
                }));
            }
        }

        private RelayCommand logout;
        public RelayCommand LogOutCommand
        {
            get
            {
                return logout ?? (logout = new RelayCommand(obj =>
                {
                    LogOut();
                }));
            }
        }

        private RelayCommand closeregister;
        public RelayCommand CloseRegisterCommand
        {
            get
            {
                return closeregister ?? (closeregister = new RelayCommand(obj =>
                {
                    CloseRegister();
                }));
            }
        }

        private RelayCommand openregister;
        public RelayCommand OpenRegisterCommand
        {
            get
            {
                return openregister ?? (openregister = new RelayCommand(obj =>
                {
                    OpenRegister();
                }));
            }
        }

        private RelayCommand register;
        public RelayCommand RegisterCommand
        {
            get
            {
                return register ?? (register = new RelayCommand(obj =>
                {
                    var values = (object[])obj;
                    string name = (string)values[0];
                    string sername = (string)values[1];
                    DateTime? dateofbirth = (DateTime?)values[2];
                    string login = (string)values[3];
                    string pass = (string)values[4];
                    PasswordBox passcheck = (PasswordBox)values[5];
                    if (name == null || sername  == null || dateofbirth == null || login == null || passcheck == null)
                    {
                        var mb = new Windows.CustomMessageBox("Не все данные введены", "Ошибка регистрации");
                        mb.ShowDialog();
                    }
                    else
                        Register(name, sername, (DateTime)dateofbirth, login, pass, passcheck);
                }));
            }
        }
        #endregion

        public AuthViewModel(IAutorizationService ass, MainViewModel mainWindow)
        {
            MainText = "Войдите в систему";
            
            authService = ass;
            var u = authService.GetCurrentUser();

            if (u.type == UserType.Unauthorized)
            {
                MainText = "Войдите в систему";
                IsAuth = Visibility.Visible;
                IsReg = Visibility.Collapsed;
                IsIn = Visibility.Collapsed;
            }
            else
            {
                MainText = "Добро пожаловать, " + u.Name + '\n' + u.Status;
                IsAuth = Visibility.Collapsed;
                IsReg = Visibility.Collapsed;
                IsIn = Visibility.Visible;
            }

            this.mainWindow = mainWindow;
        }


 
        public void Authorization(string login, string password)
        {
            if (authService.TryAuthorization(login, password))
            {
                var u = authService.GetCurrentUser();
                MainText = "Добро пожаловать, " + u.Name + '\n' + u.Status;
                IsAuth = Visibility.Hidden;
                IsReg = Visibility.Hidden;
                IsIn = Visibility.Visible;
                mainWindow.UpdateAuth();
            }
            else
            {
                var mb = new Windows.CustomMessageBox("Неверный логин или пароль", "Ошибка авторизации");
                IsAuth = Visibility.Visible;
                IsReg = Visibility.Hidden;
                IsIn = Visibility.Hidden;
                mb.ShowDialog();

            }

        }

        public void LogOut()
        {
            authService.LogOut();
            IsAuth = Visibility.Visible;
            IsReg = Visibility.Hidden;
            IsIn = Visibility.Hidden;
            MainText = "Войдите в систему";
            mainWindow.UpdateAuth();
        }

        public void OpenRegister()
        {
            IsAuth = Visibility.Collapsed;
            IsIn = Visibility.Collapsed;
            IsReg = Visibility.Visible;
            MainText = "Создайте аккаунт";
        }

        public void CloseRegister()
        {
            MainText = "Войдите в систему";
            IsAuth = Visibility.Visible;
            IsReg = Visibility.Collapsed;
            IsIn = Visibility.Collapsed;
        }

        public void Register(string name, string sername, DateTime dateofbirth, string login, string pass, PasswordBox passcheck)
        {
            if (pass == passcheck.Password)
            {
                authService.CreateCustomer(name, sername, dateofbirth, login, pass);
                Authorization(login, pass);
            }
            else
            {
                var mb = new Windows.CustomMessageBox("Пароли не совпаадают", "Ошибка регистрации");
                mb.ShowDialog();
            }
        }
    }
}
