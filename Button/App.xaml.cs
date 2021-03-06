﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Button.Account;
using Button.Core.DependencyInjection;
using Button.Core.MessageServices;
using Button.Core.Navigation;
using Button.Services;
using Button.SqlLinkService;
using Button.Views;

namespace Button
{
    /// <summary>
    /// Обеспечивает зависящее от конкретного приложения поведение, дополняющее класс Application по умолчанию.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Инициализирует одноэлементный объект приложения.  Это первая выполняемая строка разрабатываемого
        /// кода; поэтому она является логическим эквивалентом main() или WinMain().
        /// </summary>
        public App()
        {
           
            ServiceLocator.Locator.Bind<IMessageService, MessageService>(LifetimeMode.Singleton);
            ServiceLocator.Locator.Bind<INavigationService, NavigationService>(LifetimeMode.Singleton);
            ServiceLocator.Locator.Bind<IAccountService, AccountService>(LifetimeMode.Singleton);
            ServiceLocator.Locator.Bind<ISqlLinkService, LinkService>(LifetimeMode.Singleton);
            ServiceLocator.Locator.Bind<IFileStorage, FileStorage>(LifetimeMode.Singleton);

            var fileStorage = new FileStorage();
            var linkService = ServiceLocator.Locator.Get<ISqlLinkService>();

            ServiceLocator.Locator.Bind<IImageService, IImageService>(new ImagesService(linkService, fileStorage));
            UnhandledException += OnUnhandledException;

            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }
                
        private async void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            //TODO: Make logger
        }
        /// <summary>
        /// Вызывается при обычном запуске приложения пользователем.  Будут использоваться другие точки входа,
        /// например, если приложение запускается для открытия конкретного файла.
        /// </summary>
        /// <param name="e">Сведения о запросе и обработке запуска.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
               // this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Не повторяйте инициализацию приложения, если в окне уже имеется содержимое,
            // только обеспечьте активность окна
            if (rootFrame == null)
            {
                // Создание фрейма, который станет контекстом навигации, и переход к первой странице
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Загрузить состояние из ранее приостановленного приложения
                }

                // Размещение фрейма в текущем окне
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Если стек навигации не восстанавливается для перехода к первой странице,
                // настройка новой страницы путем передачи необходимой информации в качестве параметра
                // параметр
                var navigationService = ServiceLocator.Locator.Get<INavigationService>();
                navigationService.Initialize(rootFrame, GetPageResolver());
                rootFrame.Navigate(typeof(Views.MainPage), e.Arguments);
            }
            // Обеспечение активности текущего окна
            Window.Current.Activate();
        }

        private PageResolver GetPageResolver()
        {
            var dictionary = new Dictionary<string, Type>();
            dictionary.Add(ViewLocator.Drawbacks, typeof(DrawbacksPage));
            dictionary.Add(ViewLocator.Login, typeof(LoginPage));
            dictionary.Add(ViewLocator.Benefits, typeof(BenefitsPage));
            dictionary.Add(ViewLocator.Main, typeof(Views.MainPage));
            dictionary.Add(ViewLocator.MainSearch, typeof(MainSearchPage));
            dictionary.Add(ViewLocator.Profile, typeof(ProfilePage));
            dictionary.Add(ViewLocator.SelfProfile, typeof(SelfProfilePage));
            dictionary.Add(ViewLocator.Settings, typeof(SettingsPage));
            dictionary.Add(ViewLocator.Replies, typeof (RepliesPage));
            dictionary.Add(ViewLocator.SignUp, typeof (SignUpPage));
            dictionary.Add(ViewLocator.AccountActivation, typeof (AccountActivationSearch));
            return new PageResolver(dictionary);
        }

        /// <summary>
        /// Вызывается в случае сбоя навигации на определенную страницу
        /// </summary>
        /// <param name="sender">Фрейм, для которого произошел сбой навигации</param>
        /// <param name="e">Сведения о сбое навигации</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Вызывается при приостановке выполнения приложения.  Состояние приложения сохраняется
        /// без учета информации о том, будет ли оно завершено или возобновлено с неизменным
        /// содержимым памяти.
        /// </summary>
        /// <param name="sender">Источник запроса приостановки.</param>
        /// <param name="e">Сведения о запросе приостановки.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Сохранить состояние приложения и остановить все фоновые операции
            deferral.Complete();
        }
    }
}
