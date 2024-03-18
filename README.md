Teste Prático - Desenvolvedor Web

Repositório de Arquivos: Preferencial GIT
Objetivo: Avaliar o conhecimento do candidato em estrutura e boas pratica de

desenvolvimento.

Desenvolvimento: Criar uma API usando .NET Core que seja capaz de manipular as informações

de um tipo específico de dados denominado “Cliente”.
O cliente deve possuir necessariamente:  
 Id Incremental (1,1)
 Nome String
 *E-mail String – somente válidos em seu formato
 *CPF String – somente válidos em seu formato e tamanho
 *RG String – somente válidos em seu formato e tamanho
 Contatos (Objeto)
o Id Incremental (1,1)
o Tipo String (Residencial, Comercial, Celular)
o DDD Int
o Telefone Decimal
 Endereços (Objeto)
o Id Incremental (1,1)
o Tipo String (Preferencial, Entrega, Cobrança)
o *CEP String – somente válidos em seu formato e
tamanho
o Logradouro String
o Número Int
o Bairro String
o Complemento String
o Cidade String
o Estado String
o Referência String

A API deverá usar como base de dados um JSON criado por você.
Você deve pensar em um formato onde você possa escrever e recuperar esses
dados, utilizando todas as melhores práticas de desenvolvimento:
 Orientação a Objeto
 SOLID
 Injeção de Dependência
 Estrutura em Camadas
 entre outras de seu conhecimento
Validar os dados de entrada de acordo com seu tipo
Validar os dados de entrada especializados identificados por “*”.
Os serviços deverão conter as seguintes funcionalidades:  
 Listar todos os clientes cadastrados
o Com filtros, opcionais, de nome e/ou e-mail e/ou CPF.
 Adicionar Novo Cliente
 Atualizar Cliente Existente

o Incluir/Atualizar Contato ao Cliente Existente
o Incluir/Atualizar Endereço ao Cliente Existente
 Remover Cliente
o Remover Contato Existente
o Remover Endereço Existente

A API deve contemplar as rotas de acesso para as funcionalidades de serviço
definidas e os verbos http correspondentes.
 /cliente/listar
o Criado para atender lista de todos e filtros
 /cliente/criar
 /cliente/atualizar/{identificação}
 /cliente/remover/{identificação}

Build A aplicação não deve ter sua compilação comprometida.

Publicação

A aplicação deve estar configurada para publicação via localhost no Visual
Studio

Disponibilização A aplicação deverá ser disponibilizada
