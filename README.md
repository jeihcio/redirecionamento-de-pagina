# Redirecionamento de página

Essa API serve para registrar os acessos que os seus usuários tem aos links que você disponibiliza.<br/> 
Para usar basta você enviar um link para o seu usuário da seguinte forma:

http://www.seusite.com/Redirect?url=[link_para_o_redirecionamento]

Quando o seu usuário clicar no link ele será redirecionado para o site que você deseja, mas antes irá registrar no banco de dados o acesso dele, assim desta forma você conseguirá ter estatísticas de como estão sendo visitados os links que você divulga. 

## Configuração

Alterar os seguintes trechos de código:

- Em [DataContext.cs](https://github.com/jeihcio/redirecionamento-de-pagina/blob/main/Redirect.Domain/Database/Context/DataContext.cs) alterar o construtor da classe **DataContext()** adicionando a sua string de conexão no **base("[STRING DE CONEXÃO]")**

- Em [RedirectController.cs](https://github.com/jeihcio/redirecionamento-de-pagina/blob/main/Redirect/Controllers/RedirectController.cs) alterar a variável **defaultWebsiteUrl** que fica dentro do método **Index**, nela você deve informar o link padrão do seu site, por exemplo "http://www.exemplo.com".

## Estatísticas

#### [redirect/GetStatistic]
Exibe os dados referentes a uma url que você divulgou
  
  - URL
  - Quantidade de acessos únicos
  - Quantidade de acesso no geral

#### [redirect/GetStatistic/All]
Exibe a quantidade total de acessos a todos os links divulgados 

  - URL
  - Quantidade de acessos únicos
  - Quantidade de acesso no geral

#### [redirect/GetStatistic/All/Database]
Exibe todos os acessos de todos os links. Para cada acesso será mostrado:

 - Id
 - Date
 - Url
 - Guid 

#### [redirect/GetStatistic/All/Database/Distinct]
Exibe todos os links que você divulgou

- URL

#### [redirect/GetStatistic/All/Database/Distinct2]
Exibe todos os links que você divulgou
  
  - URL
  - AcessoUnico
  - AcessoGeral

### [redirect/all]
Limpar o banco de dados

## AVISO IMPORTANTE 
Se for usar comercialmente colocar controle de acesso nos recursos de estatística e de limpar o banco de dados! 
