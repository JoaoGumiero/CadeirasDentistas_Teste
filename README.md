# 🚀 CadeireasDentistas-API

![Build Status](https://img.shields.io/badge/build-passing-brightgreen)
![License](https://img.shields.io/badge/license-MIT-blue)

---

## 📋 Índice
- [Descrição](#descrição)
- [Tecnologias](#tecnologias)
- [Instalação](#instalação)
- [Uso](#uso)
- [Endpoints](#endpoints)
- [Adições posteriores](#adições)

---

## Descrição
Teste técnico de Cadeira de Dentistas em .Net. Envolve uma aplicação com operações CRUD e outras funcionalidade como a auto alocação de cadeiras para dentistas, e tudo em um ambiente modularizado.

---

## Tecnologias
- .Net 8
- Docker
- MySQL
- Swagger para documentação

---
## Pré-Requsitos

- Certifique-se de ter o Docker e o Docker Compose instalados no seu ambiente (utilizei o Docker Desktop para visualização).

## Instalação
    
1. Clone este repositório e entre na pasta pelo cmd:
- git clone https://github.com/JoaoGumiero/CadeirasDentistas_Teste.git
- cd .\CadeirasDentistas\
   
4. Inicie os serviços com Docker Compose:
    docker-compose up --build (Lembrando que necessita que o docker desktop esteja aberto)

5. Acesse a aplicação:
   A API estará disponível em: http://localhost:8080
   | Swagger disponível no endpoint: http://localhost:8080/index.html
   


## Adições Posteriores

- Poderia implementar algumas outras funcionalidades dentro do código para deixa-lo o mais realista possível como "Status de Cadeira e Alocação" e outros. Porém não foram implementados a fim de não fugir do escopo do projeto/teste.

- Poderia melhorar a captura de logs retornando uma lista de variáveis inválidas de uma vez só.
    - Teve casos (Data por exemplo) que não conseguir enviar mais de 1 variável no throw.
    
- Poderia melhorar a questão de variáveis de ambiente, colocar Prod e Dev (Ajustar os detalhes do midleware para logs da aplicação)

- Poderia tornar classes DTO mais complexas para controlar o fluxo de dados de um model que entra e que sai na resposta para o cliente.

- Incluir Design Patterns de Factory e Singleton para acesso ao banco de dados.
