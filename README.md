# Netcon GeoAPI - Projeto Viabilidade

API REST desenvolvida em .NET 8 para cálculo de viabilidade técnica de elementos geográficos (CTOs) baseada em localização e raio de cobertura.

## 🚀 Tecnologias Utilizadas

- **C# / .NET 8** (Minimal APIs)
- **EF Core In-Memory** (Para alta performance de leitura)
- **FluentValidation** (Validação de dados de entrada)
- **Docker** (Conteinerização)
- **Fórmula de Haversine** (Cálculo preciso de distância geodésica)

## 🏗️ Arquitetura

O projeto segue os princípios de **Domain-Driven Design (DDD)**, organizado nas seguintes camadas:

1.  **Presentation (Web)**: Endpoints e Middlewares.
2.  **Application**: Lógica de negócio, Serviços e DTOs.
3.  **Domain**: Entidades, Interfaces de Repositório e Regras de Negócio.
4.  **Infrastructure**: Implementação de Repositórios, Contexto de Banco e Logging.

## 🛠️ Funcionalidades Principais

- **Carregamento Automático**: O dataset JSON é importado para o banco em memória apenas uma vez no startup.
- **Filtros de Status**: Apenas elementos `ACTIVE` e `RESERVED` são considerados.
- **Log com Rotação**: Logs de Request, Tempo de Execução e Erros com limite de 1MB por arquivo.
- **Tratamento Global de Erros**: Respostas JSON padronizadas para exceções não tratadas.
- **Paginação**: Suporte nativo para 20 registros por página.

## 🐳 Como Executar via Docker

Certifique-se de ter o Docker instalado em sua máquina.

1.  **Build da Imagem**:
    ```bash
    docker build -t netcon-geo-api .
    ```

2.  **Execução do Container**:
    ```bash
    docker run -p 8080:8080 netcon-geo-api
    ```
    *A API estará disponível em: `http://localhost:8080`*

## 🛣️ Endpoints

### 1. Viabilidade Geográfica
Retorna os elementos dentro do raio especificado, ordenados por proximidade.

- **URL**: `/api/feasibility`
- **Método**: `GET`
- **Parâmetros**:
    - `latitude` (double): Latitude do ponto central.
    - `longitude` (double): Longitude do ponto central.
    - `radius` (double): Raio de busca em metros.
    - `page` (int, opcional): Número da página (padrão 1).

**Exemplo de Requisição**:
`GET http://localhost:8080/api/feasibility?latitude=-22.9101&longitude=-43.1829&radius=5000&page=1`

### 2. Health Check
Verifica o status da aplicação.

- **URL**: `/health`
- **Método**: `GET`

## 📁 Estrutura de Logs

Os logs são gerados automaticamente na pasta `/app/logs` dentro do container. Caso ocorra erro ou o arquivo atinja 1MB, um novo arquivo será gerado no formato:
- `api_log.txt` (Log atual)
- `api_log_202604172210.txt` (Arquivo rotacionado)

## 🧪 Validations

A API utiliza **FluentValidation** para garantir que:
- Latitude esteja entre -90 e 90.
- Longitude esteja entre -180 e 180.
- Raio seja um valor positivo.

---
Desenvolvido por Felipe Amorim - 2026