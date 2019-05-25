# ScopeSignalControl
New version of scope view for WPF

Allow to Scope double type signals in realTime.

After instance, some configs are avaliable:

Speed, resolution, definition...

ScopeView scopeView = new ScopeView();


How to use:
instantiate ScopeView and add to child of another view.

    ScopeView scopeView = new ScopeView();
    mainContainer.Children.Add(scopeView);
    
    After that the values setted on scopeview.inputValue will be represented in a Scope Screen in real time.
    
    scopeView.inputValue = 9.3  <--some double value wich change along time
    
    
