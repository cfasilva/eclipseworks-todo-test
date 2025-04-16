# eclipseworks-todo-test
O time de desenvolvimento de uma empresa precisa de sua ajuda para criar um sistema de gerenciamento de tarefas. O objetivo é desenvolver uma API que permita aos usuários organizar e monitorar suas tarefas diárias, bem como colaborar com colegas de equipe.

### Detalhes do App
**Usuário**

Pessoa que utiliza o aplicativo detentor de uma conta.

**Projeto**

Um projeto é uma entidade que contém várias tarefas. Um usuário pode criar, visualizar e gerenciar vários projetos.

**Tarefa**

Uma tarefa é uma unidade de trabalho dentro de um projeto. Cada tarefa possui um título, uma descrição, uma data de vencimento e um status (pendente, em andamento, concluída).

---

### Fase 1: API Coding
Para a primeira Sprint, foi estipulado o desenvolvimento de funcionalidades básicas para o gerenciamento de tarefas. Desenvolva uma RESTful API capaz de responder a requisições feitas pelo aplicativo para os seguintes itens:

1. **Listagem de Projetos** - listar todos os projetos do usuário
2. **Visualização de Tarefas** - visualizar todas as tarefas de um projeto específico
3. **Criação de Projetos** - criar um novo projeto
4. **Criação de Tarefas** - adicionar uma nova tarefa a um projeto
5. **Atualização de Tarefas** - atualizar o status ou detalhes de uma tarefa
6. **Remoção de Tarefas** - remover uma tarefa de um projeto

**Regras de negócio:**

1. **Prioridades de Tarefas:**
    - Cada tarefa deve ter uma prioridade atribuída (baixa, média, alta).
    - Não é permitido alterar a prioridade de uma tarefa depois que ela foi criada.
2. **Restrições de Remoção de Projetos:**
    - Um projeto não pode ser removido se ainda houver tarefas pendentes associadas a ele.
    - Caso o usuário tente remover um projeto com tarefas pendentes, a API deve retornar um erro e sugerir a conclusão ou remoção das tarefas primeiro.
3. **Histórico de Atualizações:**
    - Cada vez que uma tarefa for atualizada (status, detalhes, etc.), a API deve registrar um histórico de alterações para a tarefa.
    - O histórico de alterações deve incluir informações sobre o que foi modificado, a data da modificação e o usuário que fez a modificação.
4. **Limite de Tarefas por Projeto:**
    - Cada projeto tem um limite máximo de 20 tarefas. Tentar adicionar mais tarefas do que o limite deve resultar em um erro.
5. **Relatórios de Desempenho:**
    - A API deve fornecer endpoints para gerar relatórios de desempenho, como o número médio de tarefas concluídas por usuário nos últimos 30 dias.
    - Os relatórios devem ser acessíveis apenas por usuários com uma função específica de "gerente".
6. **Comentários nas Tarefas:**
    - Os usuários podem adicionar comentários a uma tarefa para fornecer informações adicionais.
    - Os comentários devem ser registrados no histórico de alterações da tarefa.

**Regras da API e avaliação:**

1. **Não é** **necessário** nenhum tipo de CRUD para usuários.
2. **Não é necessário** nenhum tipo de autenticação; este será um serviço externo.
3. Tenha pelo menos **80%** de cobertura de testes de unidade para validar suas regras de negócio.
4. **Utilize o git** como ferramenta de versionamento de código.
5. **Utilize um banco de dados** (o que preferir) para salvar os dados.
6. **Utilize o framework e libs** que julgue necessário para uma boa implementação.
7. **O projeto deve executar no docker e as informações de execução via terminal devem estar disponíveis no [README.md](http://README.md) do projeto**

---

### Fase 2: Refinamento - Perguntas para o Product Owner (PO)

Para evoluir o produto de forma alinhada às necessidades reais dos usuários e do negócio, aqui estão algumas perguntas estratégicas que eu faria ao PO:

1. **Autenticação e Perfis:**
   - Há planos para implementar autenticação de usuários (login/senha, OAuth, etc.)?
   - Quais papéis de usuário devem existir além de "admin" e "user"?

2. **Notificações e Alertas:**
   - Os usuários devem ser notificados sobre prazos, atualizações ou comentários em tarefas?
   - Há algum canal preferido para notificações (e-mail, push, SMS)?

3. **Tarefas Recorrentes ou Dependentes:**
   - Existe a necessidade de tarefas com recorrência ou dependência entre elas?

4. **Dashboard ou Kanban:**
   - É desejado um dashboard visual com gráficos de progresso, status de tarefas ou um board estilo Kanban?

5. **Filtros e Ordenações:**
   - Que tipos de filtros e ordenações os usuários gostariam para visualizar tarefas e projetos?

6. **Integrações externas:**
   - Há interesse em integrar a API com ferramentas externas (ex: Slack, Jira, Trello)?

7. **Histórico e Auditoria:**
   - Devemos manter o histórico completo de cada ação do usuário (inclusive deleções)?
   - Existe exigência legal de retenção de dados?

8. **Performance e Escalabilidade:**
   - Existe uma expectativa de carga alta? Quantos usuários e tarefas o sistema deve suportar?

9. **Internacionalização:**
   - O sistema deve suportar múltiplos idiomas?

10. **Segurança:**
    - Algum requisito específico de segurança como criptografia de dados sensíveis ou logs de acesso?

Essas perguntas ajudam a criar um roadmap técnico sólido e sustentável.

---

### Fase 3: Final – O que eu melhoraria no projeto

Durante o desenvolvimento do desafio, identifiquei oportunidades de evolução técnica e arquitetural para preparar o sistema para produção e escalabilidade. Aqui estão algumas melhorias que eu implementaria em uma próxima versão:

### 🧱 Arquitetura

- **Camada de Application Services separada**: separaria a lógica de orquestração de regras de negócio da camada de infraestrutura e de domínio.
- **Uso de CQRS com MediatR**: para separar comandos de leitura e escrita, e melhorar rastreabilidade e testabilidade.
- **Clean Architecture completa**: com inversão total de dependências, evitando referências diretas da Domain Layer a Entity Framework.

### ☁️ Cloud e DevOps

- **Deploy contínuo em nuvem (Azure ou AWS)** com pipeline no GitHub Actions
- **Banco relacional gerenciado** (ex: Azure SQL, RDS)
- **Logs estruturados com Serilog + Application Insights**
- **Monitoramento com HealthChecks e Prometheus + Grafana (em cluster)**

### 🔒 Segurança

- Implementar **autenticação JWT com refresh token**
- Proteção por **Claims** e **Role-based Authorization**
- Criptografia de dados sensíveis e logs de acesso

### 📈 Escalabilidade

- **Paginação, ordenação e filtros** eficientes nas listagens
- Utilização de **caching distribuído** com Redis
- Preparação para uso com **banco particionado ou multi-tenant**

### 💡 Experiência do Usuário

- Adicionar suporte a **comentários em tempo real** com SignalR
- Implementar uma **interface web simples** (ex: React + Tailwind) integrada à API

Essas melhorias tornariam a aplicação robusta, preparada para uso real em produção e pronta para crescer com segurança e qualidade.

---

### Como utilizar este projeto
Após clonar o repositório, este comando irá ler as configurações do arquivo `docker-compose.yml` e executar os serviços
```
docker-compose up --build
```

Verifique se a documentação da API está disponível com `Swagger`
```
http://localhost:5050/swagger/index.html
```

## ✅ Tecnologias Utilizadas

- ASP.NET Core 8
- Entity Framework Core
- InMemoryDatabase (para testes)
- xUnit + Moq (testes unitários)
- Docker
- Swagger

## 🧪 Testes

### Executar testes e gerar cobertura:

```bash
dotnet test /p:CollectCoverage=true
```

### Cobertura atual:

> ✅ Aproximadamente **+90%** de cobertura nas regras de negócio:

- ProjectService: criação, listagem, deleção com/sem pendências
- TaskService: criação com limite, atualização com histórico, bloqueio de prioridade
- ReportsController: filtragem por role
- TaskController & ProjectController: cobertura completa de endpoints

## ✨ Autor

Desenvolvido por **Cleber Abreu** para a vaga de Desenvolvedor Back-End .NET Sênior na Eclipseworks.