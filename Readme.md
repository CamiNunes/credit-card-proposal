Requisito:

Segue abaixo as instruções para realização da atividade:

O seu objetivo é propor a comunicação de 3 microserviços por mensageria.

Situação problema:

Dado o cadastro de um novo cliente, deverá existir uma comunicação por mensageria:

1. Geração de uma proposta de crédito;
2. Emissão de 1 ou mais cartões de crédito;


Pensando que pode haver erros de comunicação na proposta de crédito ou na emissão do cartão, deverá existir um processo de resiliência e uma sinalização via evento ao microserviço de clientes.


Microserviços:

1. Cadastro de clientes;
2. Proposta de crédito;
3. Cartão de crédito;

Requisitos técnicos:

1. Proposta deverá ter como base o .Net 8.0;
2. Serviço de mensageria deve ter como base, RabbitMQ, Azure services bus e/ou Azure eventhub

Entrega:

Você deverá entregar um documento com uma proposta de solução em PDF, se desejar, pode adicionar imagens dos fluxos usando como base o site https://draw.io/.


Estrutura de Comunicação:

1. Cadastro de Cliente:
    - Quando um novo cliente é cadastrado, uma mensagem é enviada para a fila que o ‘Customer.Worker’;
2. Proposta de Crédito:
    - O ‘Customer.Worker’ processa a mensagem e envia uma proposta de crédito para a fila que o ‘CreditProposal.Worker’ está escutando.
    - O ‘CreditProposal.Worker’ valida a proposta de crédito. Se a proposta for aprovada, ele envia uma mensagem para a fila que o
    
    ‘RequestCreditCard.Worker’ está escutando. Caso contrário, ele envia uma mensagem de rejeição de volta para o serviço de ‘Cadastro de Cliente’.
    
3. Emissão de Cartão de Crédito:
- O ‘RequestCreditCard.Worker’ emite o cartão de crédito e envia uma mensagem de confirmação para o serviço de ‘Cadastro de Cliente’.

- Fluxograma do sistema:
  
<div align="center">
    <img src="./fluxograma.png" alt="Diagrama da Arquitetura">
</div>


## Instruções para rodar o projeto:

- Instalação do RabbitMQ:
    - ```sh
        docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
        ```
- Instalação do MongoDB:
    - ```sh
        docker run -d --name mongodb -p 27017:27017 mongo
        ``` 
- Iniciar os Serviços e a API:

1. **Customer.Worker**
    - Navegue até o diretório do projeto Customer.Worker e execute os comandos:
        ```sh
        cd src/Services/Customer.Worker
        dotnet restore
        dotnet build
        dotnet run
        ```

2. **CreditProposal.Worker**
    - Navegue até o diretório do projeto CreditProposal.Worker e execute os comandos:
        ```sh
        cd src/Services/CreditProposal.Worker
        dotnet restore
        dotnet build
        dotnet run
        ```

3. **RequestCreditCard.Worker**
    - Navegue até o diretório do projeto RequestCreditCard.Worker e execute os comandos:
        ```sh
        cd src/Services/RequestCreditCard.Worker
        dotnet restore
        dotnet build
        dotnet run
        ```

4. **ProposalCreditCard.API**
    - Navegue até o diretório do projeto ProposalCreditCard.API e execute os comandos:
        ```sh
        cd src/Modules/ProposalCreditCard.API
        dotnet restore
        dotnet build
        dotnet run
        ```


