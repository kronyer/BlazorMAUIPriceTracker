using Microsoft.AspNetCore.Components;

namespace PriceTracker.Services
{
    public class AppSharedState
    {


        public Action? BackButtonPressed;
        private readonly Stack<string> _navigationStack = new();
        private WeakReference<IBackButtonHandler>? _currentHandler;
        private NavigationManager _nav;

        private readonly HashSet<string> _ignoredRoutes = new(StringComparer.OrdinalIgnoreCase)
        {
            "/register-price"
        };


        public void SetNavigationManager(NavigationManager nav)
        {
            _nav = nav;
        }

        public void RegisterBackButtonHandler(IBackButtonHandler handler)
        {
            _currentHandler = new WeakReference<IBackButtonHandler>(handler);
        }

        public async Task RaiseBackButtonPressedAsync()
        {
            if (_currentHandler?.TryGetTarget(out var handler) == true)
            {
                await handler.OnBackButtonPressedAsync();
            }
            else
            {
                var previous = PopValidRoute();
                if (previous != null)
                {
                    _nav.NavigateTo(previous);
                }
                else
                {
                    bool confirmed = await Application.Current.MainPage.DisplayAlert(
                        "Sair do app?",
                        "Deseja realmente fechar o aplicativo?",
                        "Sim", "Cancelar"
                    );

                    if (confirmed)
                        Application.Current.Quit();
                }
            }
        }


        public void PushRoute(string uri)
        {
            // Não ignora mais nenhuma rota
            if (_navigationStack.Count == 0 || _navigationStack.Peek() != uri)
                _navigationStack.Push(uri);
        }

        public string? PopValidRoute()
        {
            while (_navigationStack.Count > 1)
            {
                _navigationStack.Pop(); // remove o atual
                var previous = _navigationStack.Peek();

                if (!_ignoredRoutes.Any(ignored => previous.StartsWith(ignored, StringComparison.OrdinalIgnoreCase)))
                    return previous;
            }

            return null;
        }


        public string? PeekPreviousRoute()
        {
            if (_navigationStack.Count > 1)
            {
                var current = _navigationStack.Pop();
                var previous = _navigationStack.Peek();
                _navigationStack.Push(current);
                return previous;
            }
            return null;
        }
    }
}

public interface IBackButtonHandler
{
    Task OnBackButtonPressedAsync();
}

