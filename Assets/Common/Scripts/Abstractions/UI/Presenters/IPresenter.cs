﻿using KarenKrill.Common.UI.Views.Abstractions;

namespace KarenKrill.Common.UI.Presenters.Abstractions
{
    public interface IPresenter
    {
        void Enable();
        void Disable();
    }
    public interface IPresenter<T> : IPresenter where T : IView
    {
        /// <summary>
        /// <see cref="View"/> property must be set before calling <see cref="IPresenter.Enable"/> or <see cref="IPresenter.Disable"/>
        /// </summary>
        T View { get; set; }
    }
}