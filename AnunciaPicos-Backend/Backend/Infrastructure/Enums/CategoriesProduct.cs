using System.ComponentModel;

namespace AnunciaPicos.Backend.Infrastructure.Enums
{
    public enum CategoriesProduct
    {
        others = 0,            // Para qualquer produto que não se encaixe nas outras categorias
        electronics = 1,       // Eletrônicos (ex.: smartphones, computadores, TVs)
        furniture = 2,         // Móveis (ex.: sofás, camas, mesas)
        clothing = 3,          // Roupas (ex.: camisetas, vestidos, calças)
        home_appliances = 4,   // Eletrodomésticos (ex.: geladeiras, micro-ondas, liquidificadores)
        books = 5,             // Livros (ex.: literatura, educação, ficção)
        sports = 6,            // Produtos esportivos (ex.: equipamentos de ginástica, roupas esportivas)
        beauty_health = 7,     // Produtos de beleza e saúde (ex.: cosméticos, suplementos, perfumes)
        toys = 8,              // Brinquedos (ex.: jogos, brinquedos infantis)
        automotive = 9,        // Automotivo (ex.: peças de carros, acessórios automotivos)
        garden = 10,           // Produtos para jardim e ao ar livre (ex.: ferramentas de jardinagem, móveis para jardim)
        food_beverages = 11    // Alimentos e bebidas (ex.: snacks, bebidas, alimentos gourmet)
    }
}
