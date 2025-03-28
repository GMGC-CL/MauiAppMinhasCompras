using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            lista.Clear();
            List<Produto> tmp = await App.Db.GetAll();
            tmp.ForEach(i => lista.Add(i));

            var categorias = tmp.Select(p => p.Categoria).Distinct().ToList();
            pck_categoria.ItemsSource = categorias;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());

        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;

            lst_produtos.IsRefreshing = true;

            lista.Clear();

            List<Produto> tmp = await App.Db.Search(q);

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);

        string msg = $"O total � {soma:C}";

        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    private void ToolbarItem_Clicked_2(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.RelatorioCategoria());

        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecinado = sender as MenuItem;

            Produto p = selecinado.BindingContext as Produto;

            bool confirm = await DisplayAlert(
                "Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "N�o");

            if(confirm)
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto(p));
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void FiltrarProdutos(object sender, EventArgs e)
    {
        try
        {
            DateTime inicio = dtp_inicio.Date;
            DateTime fim = dtp_fim.Date;
            string categoriaSelecionada = pck_categoria.SelectedItem as string;
            bool? comprado = chkComprado.IsChecked;
            bool? ncomprado = chkNComprado.IsChecked;

            lista.Clear();
            List<Produto> tmp;

            
            tmp = await App.Db.GetProdutosFiltrados(categoriaSelecionada, comprado, ncomprado, inicio, fim);

            Debug.WriteLine($"Produtos encontrados: {tmp.Count}");
            tmp.ForEach(p =>
                Debug.WriteLine($"Produto: {p.Descricao}, Comprado: {p.Comprado}, DataCompra: {p.DataCompra}")
            );

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private async void LimparFiltros(object sender, EventArgs e)
    {
        try
        {
            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();
            tmp.ForEach(i => lista.Add(i));
            dtp_inicio.Date = DateTime.Today;
            dtp_fim.Date = DateTime.Today;
            pck_categoria.SelectedItem = null;
            chkComprado.IsChecked = false;
            chkNComprado.IsChecked = false;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}