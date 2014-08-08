using Android.OS;
using Android.Support.V4.App;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Fragging.Fragments.EventSource;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.Fragging.Fragments
{
    public class MvxFragmentAdapter 
        : MvxBaseFragmentAdapter
    {
        public IMvxFragmentView FragmentView
        {
            get { return Fragment as IMvxFragmentView; }
        }

        public MvxFragmentAdapter(IMvxEventSourceFragment eventSource) 
            : base(eventSource)
        {
        }

        protected override void HandleCreateCalled(object sender, MvxValueEventArgs<Bundle> bundleArgs)
        {
            Bundle bundle = null;
            if (bundleArgs != null && bundleArgs.Value != null)
            {
                // saved state
                bundle = bundleArgs.Value;
            }
            else
            {
                var fragment = FragmentView as Fragment;
                if (fragment != null && fragment.Arguments != null)
                {
                    bundle = fragment.Arguments;
                }    
            }

            IMvxSavedStateConverter converter;
            if (!Mvx.TryResolve(out converter))
            {
                MvxTrace.Warning("Saved state converter not available - saving state will be hard");
            }
            else
            {
                if (bundle != null)
                {
                    var mvxBundle = converter.Read(bundle);
                    FragmentView.OnCreate(mvxBundle);
                }
            }
        }

        protected override void HandleSaveInstanceStateCalled(object sender, MvxValueEventArgs<Bundle> bundleArgs)
        {
            var mvxBundle = FragmentView.CreateSaveStateBundle();
            if (mvxBundle != null)
            {
                IMvxSavedStateConverter converter;
                if (!Mvx.TryResolve(out converter))
                {
                    MvxTrace.Warning("Saved state converter not available - saving state will be hard");
                }
                else
                {
                    converter.Write(bundleArgs.Value, mvxBundle);
                }
            }
            //var cache = Mvx.Resolve<IMvxSingleViewModelCache>();
            //cache.Cache(FragmentView.ViewModel, bundleArgs.Value);
        }
    }
}