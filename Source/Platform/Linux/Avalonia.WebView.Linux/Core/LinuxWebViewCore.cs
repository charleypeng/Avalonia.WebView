﻿using Linux.WebView.Core;

namespace Avalonia.WebView.Linux.Core;

public partial class LinuxWebViewCore : IPlatformWebView<LinuxWebViewCore>
{
    public LinuxWebViewCore(ILinuxApplication linuxApplication, ViewHandler handler, IVirtualWebViewControlCallBack callback, IVirtualBlazorWebViewProvider? provider, WebViewCreationProperties webViewCreationProperties)
    {
        _application = linuxApplication;
        _provider = provider;
        _messageKeyWord = "webview";
        _callBack = callback;
        _handler = handler;
        _creationProperties = webViewCreationProperties;

        _dispatcher = linuxApplication.Dispatcher;
        var gtkWrapper = linuxApplication.CreateWebView().Result;

        _hostWindow = gtkWrapper.Item1;
        _webView = gtkWrapper.Item2;
        NativeHandler = gtkWrapper.Item3;
        _hostWindowX11Handle = gtkWrapper.Item3;

        _userContentMessageReceived = WebView_WebMessageReceived;
        RegisterEvents();
    }

    ~LinuxWebViewCore()
    {
        Dispose(disposing: false);
    }

    delegate void void_nint_nint_nint(nint arg0, nint arg1, nint arg2);

    readonly GWindow _hostWindow;
    readonly WebKitWebView _webView;
    readonly IntPtr _hostWindowX11Handle;
    readonly ILinuxApplication _application;
    readonly ILinuxDispatcher _dispatcher; 
    readonly string _messageKeyWord;
    readonly IVirtualBlazorWebViewProvider? _provider;
    readonly IVirtualWebViewControlCallBack _callBack;
    readonly ViewHandler _handler;
    readonly WebViewCreationProperties _creationProperties;
    readonly string _dispatchMessageCallback = "__dispatchMessageCallback";

    readonly void_nint_nint_nint _userContentMessageReceived;

    WebScheme? _webScheme;

    bool _isBlazorWebView = false;

    bool _isInitialized = false;
    public bool IsInitialized
    {
        get => Volatile.Read(ref _isInitialized);
        private set => Volatile.Write(ref _isInitialized, value);
    }

    bool _isdisposed = false;
    public bool IsDisposed
    {
        get => Volatile.Read(ref _isdisposed);
        private set => Volatile.Write(ref _isdisposed, value);
    }

    public WebKitWebView WebView => _webView;

}
