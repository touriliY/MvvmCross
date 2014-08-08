// MvxFragmentExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments.EventSource;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.Fragging.Fragments
{
    public static class MvxFragmentExtensions
    {
        public static void AddEventListeners(this IMvxEventSourceFragment fragment)
        {
            if (fragment is IMvxFragmentView)
            {
                var adapter = new MvxBindingFragmentAdapter(fragment);
            }
        }

        public static void OnCreate(this IMvxFragmentView fragmentView, IMvxBundle bundle, MvxViewModelRequest request = null)
        {
            var view = fragmentView as IMvxView;
            view.OnViewCreate(() => LoadViewModel(fragmentView, bundle, request));
        }

        private static IMvxViewModel LoadViewModel(this IMvxFragmentView fragmentView, IMvxBundle savedState, MvxViewModelRequest request = null)
        {
            var viewModelType = fragmentView.FindAssociatedViewModelTypeOrNull();
            if (viewModelType == typeof(MvxNullViewModel))
                return new MvxNullViewModel();

            if (viewModelType == null
                || viewModelType == typeof (IMvxViewModel))
            {
                MvxTrace.Trace("No ViewModel class specified for {0} in LoadViewModel",
                               fragmentView.GetType().Name);
            }

            if (request == null)
                request = MvxViewModelRequest.GetDefaultRequest(viewModelType);

            var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(request, savedState);

            return viewModel;
        }

        public static void EnsureBindingContextIsSet(this IMvxFragmentView fragment, LayoutInflater inflater)
        {
            var actualFragment = (Fragment) fragment;

            if (fragment.BindingContext == null)
            {
                fragment.BindingContext = new MvxAndroidBindingContext(actualFragment.Activity,
                                                                new MvxSimpleLayoutInflater(inflater),
                                                                fragment.DataContext);
            }
            else
            {
                var androidContext = fragment.BindingContext as IMvxAndroidBindingContext;
                if (androidContext != null)
                    androidContext.LayoutInflater = new MvxSimpleLayoutInflater(inflater);
            }
        }

        public static void EnsureBindingContextIsSet(this IMvxFragmentView fragment, Bundle b0)
        {
            var actualFragment = (Fragment) fragment;

            if (fragment.BindingContext == null)
            {
                fragment.BindingContext = new MvxAndroidBindingContext(actualFragment.Activity,
                                            new MvxSimpleLayoutInflater(
                                                actualFragment.Activity.LayoutInflater),
                                            fragment.DataContext);
            }
            else
            {
                var androidContext = fragment.BindingContext as IMvxAndroidBindingContext;
                if (androidContext != null)
                    androidContext.LayoutInflater = new MvxSimpleLayoutInflater(actualFragment.Activity.LayoutInflater);
            }
        }
    }
}