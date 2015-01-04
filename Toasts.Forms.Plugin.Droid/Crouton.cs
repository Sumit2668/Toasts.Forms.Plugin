using System;
using Android.App;
using Android.Views;
using Android.Views.Animations;

namespace Toasts.Forms.Plugin.Droid
{
    public class Crouton : Java.Lang.Object, View.IOnClickListener
    {
        private readonly View _customView;
        private readonly Action _onClick;

        private Activity _activity;
        private Animation _inAnimation;
        private Animation _outAnimation;

        public long DurationInMilliseconds { get; private set; }

        public Crouton(Activity activity, View customView, int durationInMs, Action onClick = null)
        {
            if (activity == null) throw new ArgumentNullException("activity");
            if (customView == null) throw new ArgumentNullException("customView");

            _activity = activity;
            _customView = customView;
            _onClick = onClick;
            customView.SetOnClickListener(this);
            DurationInMilliseconds = durationInMs;
        }
        
        public void Show()
        {
            Manager.Instance.Add(this);
        }

        public Animation GetInAnimation()
        {
            if ((null == _inAnimation) && (null != _activity))
            {
                MeasureCroutonView();
                _inAnimation = DefaultAnimationsBuilder.BuildDefaultSlideInDownAnimation(GetView());
            }
            return _inAnimation;
        }

        public Animation GetOutAnimation()
        {
            if ((null == _outAnimation) && (null != _activity))
            {
                _outAnimation = DefaultAnimationsBuilder.BuildDefaultSlideOutUpAnimation(GetView());
            }
            return _outAnimation;
        }
        
        public bool IsShowing()
        {
            return (null != _activity) && IsCustomViewNotNull();
        }
        
        private bool IsCustomViewNotNull()
        {
            return (null != _customView) && (null != _customView.Parent);
        }

        public void DetachActivity()
        {
            _activity = null;
        }

        public Activity GetActivity()
        {
            return _activity;
        }
        
        public View GetView()
        {
            return _customView;
        }

        private void MeasureCroutonView()
        {
            View view = GetView();
            var widthSpec = View.MeasureSpec.MakeMeasureSpec(_activity.Window.DecorView.MeasuredWidth, MeasureSpecMode.AtMost);
            view.Measure(widthSpec, View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
        }

        public void OnRemoved()
        {
        }

        public void OnDisplayed()
        {
        }

        public void DetachLifecycleCallback()
        {
        }

        public void OnClick(View view)
        {
            if (_onClick != null)
                _onClick();
        }
    }
}