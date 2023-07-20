# Desafio Banco Carrefour - Arquiteto de Software

# Sobre o projeto
O objetivo deste repositório é demonstrar minha habilidades técnicas e arquiteturais em um desafio proposto para o banco carrefour

# Requisitos funcionais
* Adicionar operação de Crédito ou Débito
* Consultar saldo por dia

# Requisitos não funcionais
* Segurança
* Disponibilidade
* Resiliência
* Performance


# Tecnologias usadas

* ASP.NET Core (.NET 6) Web API
* Entity Framework Core (EFCore 6)
* MediatR for .NET 6
* Fluent Validation for .NET 6
* SQLite
* SwaggerUI
* AutoMapper
* Ocelot
* Consul
* Fortgate
* Sonar
* Prometheus
* Redis

![teste drawio](https://github.com/cassio2586/ArquiteturaReferenciaC-/assets/27103177/6cb5c765-8479-42c2-b071-162ab90f01a5)

# Porquê utilizei Microserviços
Microserviços oferecem escalabilidade, flexibilidade, agilidade no desenvolvimento, facilidade de manutenção e evolução, resiliência, escalabilidade seletiva, integração e interoperabilidade, além de facilitar a adoção de novas tecnologias. 

# Design e arquitetura dos microserviços
O design de arquitetura escolhido foi a arquitetura limpa. Ela inclui todas as estruturas, ferramentas ou recursos possíveis dos quais um aplicativo corporativo específico pode se beneficiar. Abaixo está uma lista das dependências de tecnologia que o design inclui e por que elas foram escolhidas. A maioria delas pode ser facilmente trocada pela tecnologia de sua escolha, já que a natureza dessa arquitetura é suportar modularidade e encapsulamento.

# Core
O projeto Core é o centro do projeto na arquitetura limpa e todas as outras dependências do projeto devem apontar para ele. Como tal, tem muito poucas dependências externas. A única exceção nesse caso é o pacote System.Reflection.TypeExtensions, que é usado por ValueObject para ajudar a implementar sua interface IEquatable<>. O projeto principal deve incluir coisas como:

    Entidades
    Agregados
    Eventos de domínio
    DTOs
    Interfaces
    manipuladores de eventos
    Serviços de domínio
    Especificações

# SharedKernel - Common Library(nuget)
Muitos projetos fazem referência a um projeto/pacote Shared Kernel separado. Eu recomendo criar um projeto e uma solução SharedKernel separados se você precisar compartilhar o código entre vários contextos limitados (consulte Fundamentos do DDD). Recomendo ainda que seja publicado como um pacote NuGet (provavelmente de forma privada na organização) e referenciado como uma dependência do NuGet pelos projetos que o exigem. Para este exemplo, para simplificar, adicionei um projeto SharedKernel à solução. Ele contém tipos que provavelmente seriam compartilhados entre vários contextos limitados (normalmente soluções VS), na minha experiência.


# Infrastructure 
A maioria das dependências do aplicativo com acesso a recursos externos deve ser implementada nas classes definidas no projeto de infraestrutura. Essas classes devem implementar interfaces definidas no Core. Se você tiver um projeto muito grande com muitas dependências, pode fazer sentido ter vários projetos de infraestrutura (por exemplo, Infrastructure.Data), mas para a maioria dos projetos, um projeto de infraestrutura com pastas funciona bem. A amostra inclui acesso a dados e implementações de eventos de domínio, mas você também adicionaria itens como provedores de e-mail, acesso a arquivos, clientes de API da Web etc.

O projeto de infraestrutura depende de Microsoft.EntityFrameworkCore.SqlServer e Autofac. O primeiro é usado porque está integrado aos modelos ASP.NET Core padrão e é o menor denominador comum de acesso a dados. Se desejar, ele pode ser facilmente substituído por um ORM mais leve como o Dapper. Autofac (anteriormente StructureMap) é usado para permitir que a conexão de dependências ocorra mais próximo de onde residem as implementações. Neste caso, uma classe InfrastructureRegistry pode ser utilizada na classe Infrastructure para permitir a conexão de dependências ali, sem que o ponto de entrada da aplicação precise ter uma referência ao projeto ou seus tipos. Saiba mais sobre esta técnica. A implementação atual não inclui esse comportamento - é algo que normalmente abordo e faço com que os próprios alunos adicionem em meus workshops.

# Web
O ponto de entrada do aplicativo é o projeto Web ASP.NET Core. Este é, na verdade, um aplicativo de console, com um método Main public static void em Program.cs. Atualmente, ele usa a organização MVC padrão (pastas Controllers e Views), bem como a maior parte do código de modelo de projeto ASP.NET Core padrão. Isso inclui seu sistema de configuração, que usa o arquivo appsettings.json padrão mais variáveis ​​de ambiente e é configurado em Startup.cs. O projeto delega ao projeto de Infraestrutura para conectar seus serviços usando o Autofac.


# Patterns Usados
Este modelo de solução tem código integrado para dar suporte a alguns padrões comuns, especialmente padrões de Design orientado a domínio. Aqui está uma breve visão geral de como alguns deles funcionam.

# Domain Events
Os eventos de domínio são um ótimo padrão para desacoplar um gatilho para uma operação de sua implementação. Isso é especialmente útil dentro das entidades de domínio, pois os manipuladores dos eventos podem ter dependências, enquanto as próprias entidades normalmente não. No exemplo, você pode ver isso em ação com o método ToDoItem.MarkComplete(). O diagrama de sequência a seguir demonstra como o evento e seu manipulador são usados ​​quando um item é marcado como concluído por meio de um ponto de extremidade da API da Web.


# Observabilidade - Escolha do Prometheus
1 - Coleta de métricas: O Prometheus é projetado para coletar métricas de diferentes componentes de um sistema. Ele oferece suporte a várias bibliotecas e protocolos de exportação, permitindo que você instrumente seu código para expor métricas relevantes. Isso permite que você monitore o desempenho e o comportamento do sistema em tempo real.

2 - Consultas e alertas flexíveis: O Prometheus fornece uma linguagem de consulta flexível chamada PromQL, que permite que você faça consultas poderosas sobre os dados coletados. Você pode criar consultas para calcular estatísticas, criar gráficos e detectar anomalias. Além disso, o Prometheus possui recursos integrados de alerta, permitindo que você defina regras personalizadas para acionar alertas com base nas métricas coletadas.

3 - Armazenamento eficiente: O Prometheus usa uma abordagem de armazenamento baseada em séries temporais, o que o torna eficiente em termos de espaço de armazenamento e tempo de recuperação de dados. Ele retém amostras em um formato compacto por um período de tempo configurável, o que o torna adequado para coletar dados de longo prazo. Isso significa que você pode acompanhar o histórico de métricas e analisar tendências ao longo do tempo.

4 - Ecossistema e integração: O Prometheus possui um ecossistema robusto de ferramentas e integrações que o tornam uma escolha popular para observabilidade. Ele pode ser facilmente integrado a outras ferramentas de monitoramento e alerta, como Grafana, Alertmanager e diversas bibliotecas de instrumentação. Além disso, existem bibliotecas cliente disponíveis para várias linguagens de programação, facilitando a instrumentação de seus aplicativos.

5 - Escalabilidade e suporte a alta disponibilidade: O Prometheus foi projetado para ser escalável e suportar implantações de alta disponibilidade. Você pode configurar várias instâncias do Prometheus em um ambiente de produção para coletar métricas de forma distribuída. Além disso, ele suporta sistemas de armazenamento remoto para cenários em que você precisa coletar métricas em várias regiões ou data centers.

# Resiliência - Escolha do Ocelot
   1-  Tolerância a falhas: O Ocelot fornece recursos para lidar com falhas em sistemas distribuídos. Ele oferece suporte a estratégias de fallback, que permitem que você defina um comportamento alternativo quando um serviço ou rota está indisponível. Isso significa que seu sistema pode continuar operando mesmo quando ocorrem falhas temporárias em serviços dependentes.

   2 - Circuit Breaker: O Ocelot implementa o padrão de projeto Circuit Breaker, que ajuda a evitar falhas em cascata e sobrecarga de recursos. O Circuit Breaker monitora a disponibilidade de um serviço e interrompe temporariamente as chamadas a esse serviço se ele estiver indisponível ou com mau desempenho. Isso ajuda a proteger seu sistema e permite uma rápida recuperação quando o serviço se torna disponível novamente.

   3 - Roteamento dinâmico: O Ocelot permite o roteamento dinâmico de solicitações com base em várias regras e critérios. Isso significa que você pode redirecionar as solicitações para diferentes serviços com base em condições específicas, como balanceamento de carga, localização geográfica, capacidade de resposta do serviço e muito mais. Essa flexibilidade permite que você dimensione e gerencie seus serviços de forma eficiente.

  4 -  Gerenciamento de limites: O Ocelot permite definir limites e políticas para controlar o acesso aos serviços. Você pode configurar limites de taxa por IP, por usuário ou por serviço, evitando assim abusos e sobrecargas. Além disso, o Ocelot oferece recursos de cache, que podem melhorar o desempenho do sistema, reduzindo a carga nos serviços backend.

5 - Integração com outros serviços: O Ocelot é altamente flexível e pode ser facilmente integrado com outros serviços e ferramentas. Ele pode ser usado em conjunto com plataformas de orquestração de contêineres, como Kubernetes, para fornecer resiliência em ambientes de contêiner. Além disso, o Ocelot é compatível com várias bibliotecas e padrões de autenticação e autorização, permitindo a integração perfeita com sistemas de segurança existentes.

# Performance - CQRS
Melhorar a performance com CQRS (Command Query Responsibility Segregation) pode trazer benefícios significativos para sistemas de software. Aqui estão algumas razões pelas quais a implementação de CQRS pode ajudar a melhorar a performance:

   1 - Modelagem otimizada para leitura e escrita: Com CQRS, você pode projetar modelos de leitura otimizados para consultas específicas, sem se preocupar com a consistência dos dados. Isso significa que você pode criar estruturas de dados denormalizadas, índices específicos e caches de leitura para acelerar as operações de consulta. Ao separar a lógica de leitura e escrita, você pode adotar abordagens diferentes para cada uma delas, garantindo uma melhor performance em ambos os casos.

   2 - Escalabilidade separada: Com CQRS, você pode dimensionar a parte de leitura e a parte de escrita independentemente, permitindo que cada uma seja escalada de acordo com suas necessidades específicas. A camada de leitura normalmente é mais exigida, e com CQRS você pode escalar horizontalmente essa camada adicionando mais servidores ou usando mecanismos de cache. Isso evita que operações de leitura impactem negativamente o desempenho das operações de escrita.

   3 - Cache otimizado: CQRS permite que você implemente caches de leitura para armazenar os resultados de consultas frequentes. Com a separação entre a camada de leitura e escrita, você pode facilmente manter e atualizar esses caches de forma independente, garantindo que as consultas sejam atendidas de forma mais rápida. Isso reduz a carga no sistema de banco de dados e melhora a velocidade de resposta das operações de leitura.

   4 - Redução de bloqueios e concorrência: Com a separação de responsabilidades entre comandos e consultas, você pode evitar bloqueios desnecessários no banco de dados. As operações de leitura podem ser executadas sem interferir nas operações de escrita, permitindo um melhor desempenho em cenários de concorrência. Isso é especialmente relevante em sistemas de alta carga, onde a performance é crítica.

   5 - Adoção de tecnologias específicas: Ao implementar CQRS, você pode escolher tecnologias específicas para cada parte do sistema, levando em consideração os requisitos de performance. Por exemplo, você pode optar por um banco de dados relacional otimizado para escrita e um mecanismo de indexação de alto desempenho para leitura. Isso permite a utilização das melhores ferramentas para cada contexto, maximizando a performance global do sistema.

# Segurança - Escolha Auth0
1 - Gestão de identidade e acesso: O Auth0 oferece recursos avançados de gestão de identidade e acesso, permitindo que você adicione autenticação robusta e autorização granular aos seus aplicativos. Ele suporta autenticação multi-fator (MFA), autenticação social (por exemplo, login com contas do Google, Facebook, etc.), autenticação baseada em tokens (como JWT) e muito mais. Isso permite que você proteja seus aplicativos e APIs de forma eficaz, garantindo que apenas usuários autorizados tenham acesso.

2 - Integração fácil: O Auth0 é projetado para facilitar a integração em diferentes cenários de aplicativos e plataformas. Ele oferece bibliotecas cliente para várias linguagens de programação e frameworks populares, bem como SDKs nativos para dispositivos móveis. Além disso, o Auth0 possui uma ampla gama de extensões e integrações pré-construídas para serviços populares, como AWS, Azure, WordPress, Salesforce e muitos outros, o que simplifica ainda mais a implementação de segurança em seu ecossistema de aplicativos.

3 - Gestão de usuários e conformidade: O Auth0 fornece recursos completos de gestão de usuários, permitindo que você gerencie o ciclo de vida dos usuários, defina perfis personalizados, controle o acesso com base em papéis e atributos, e muito mais. Além disso, o Auth0 oferece recursos para auxiliar na conformidade com regulamentos, como o GDPR, com ferramentas para gerenciar consentimento, retificação e exclusão de dados pessoais.

4 - Escalabilidade e disponibilidade: O Auth0 é projetado para ser altamente escalável e disponível. Ele lida com a complexidade da segurança e da autenticação, permitindo que você se concentre no desenvolvimento de seus aplicativos principais. O Auth0 é executado em uma infraestrutura globalmente distribuída e altamente resiliente, garantindo alta disponibilidade e desempenho para seus aplicativos em qualquer lugar do mundo.

5 - Segurança e conformidade robustas: O Auth0 leva a segurança a sério, adotando as melhores práticas e padrões do setor. Ele possui certificações de segurança, como ISO 27001, SOC 2 Type II, entre outras, e cumpre as regulamentações de privacidade de dados, como o GDPR e o CCPA. O Auth0 também oferece recursos avançados de segurança, como detecção de ameaças, análise de risco adaptativa e políticas de segurança personalizáveis.

# Qualidade de Código - Escolha SonarCube
O SonarQube é uma ferramenta popular para análise estática de código, oferecendo várias vantagens para melhorar a qualidade do código em poucas palavras:

  1 - Detecção de problemas: O SonarQube identifica problemas de qualidade de código, como vulnerabilidades, bugs, duplicação de código, complexidade excessiva e violações de boas práticas de programação. Isso ajuda a identificar e corrigir problemas antes que eles se tornem um obstáculo para a manutenção e evolução do software.

  2 -  Padronização: O SonarQube permite a definição de regras personalizadas para garantir a aderência a padrões de codificação específicos. Isso promove a padronização do código em toda a equipe de desenvolvimento, facilitando a legibilidade, a manutenção e a colaboração.

  3 - Melhoria contínua: Ao fornecer uma visão clara dos problemas de qualidade do código, o SonarQube incentiva uma mentalidade de melhoria contínua. Os desenvolvedores podem usar as informações fornecidas para aprender com os erros e aprimorar suas habilidades de programação ao longo do tempo.

  4 - Integração contínua: O SonarQube pode ser integrado facilmente em pipelines de integração contínua, permitindo que a análise de código seja executada automaticamente em cada nova alteração de código. Isso ajuda a identificar problemas precocemente e a evitar a propagação de problemas de qualidade em todo o código-base.

  5 -  Métricas e relatórios: O SonarQube fornece métricas e relatórios detalhados sobre a qualidade do código, permitindo que as equipes monitorem o progresso ao longo do tempo. Isso ajuda a identificar áreas de foco, definir metas de qualidade e acompanhar o impacto das melhorias implementadas.

    
