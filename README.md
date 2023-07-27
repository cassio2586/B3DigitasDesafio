# Desafio B3 Digitas - Engenheiro de Software Sênior

# Sobre o projeto
O objetivo deste repositório é demonstrar minha habilidades técnicas e arquiteturais em um desafio proposto pela B3 Digitas.

# Requisitos funcionais
* Obter OrderBook dos intrumentos BTC/USD e ETH/USD e persistir
* Demonstrar Maior e menor preço de cada ativo naquele momento
* Demonstrar Média de preço de cada ativo naquele momento
* Demonstrar Média de preço de cada ativo acumulada nos últimos 5 segundos
* Demonstrar Média de quantidade acumulada de cada ativo
* Calcular simulação de melhor preço e persistir cálculo

# Requisitos não funcionais
* Segurança
* Disponibilidade
* Resiliência
* Performance


# Tecnologias usadas

* ASP.NET Core (.NET 6) Web API
* Entity Framework Core (EFCore 6)
* MediatR for .NET 6
* SQLite
* SwaggerUI
* AutoMapper

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


# Performance - CQRS - MemoryCache .NET
Melhorar a performance com CQRS (Command Query Responsibility Segregation) pode trazer benefícios significativos para sistemas de software. Aqui estão algumas razões pelas quais a implementação de CQRS pode ajudar a melhorar a performance:

   1 - Modelagem otimizada para leitura e escrita: Com CQRS, você pode projetar modelos de leitura otimizados para consultas específicas, sem se preocupar com a consistência dos dados. Isso significa que você pode criar estruturas de dados denormalizadas, índices específicos e caches de leitura para acelerar as operações de consulta. Ao separar a lógica de leitura e escrita, você pode adotar abordagens diferentes para cada uma delas, garantindo uma melhor performance em ambos os casos.

   2 - Escalabilidade separada: Com CQRS, você pode dimensionar a parte de leitura e a parte de escrita independentemente, permitindo que cada uma seja escalada de acordo com suas necessidades específicas. A camada de leitura normalmente é mais exigida, e com CQRS você pode escalar horizontalmente essa camada adicionando mais servidores ou usando mecanismos de cache. Isso evita que operações de leitura impactem negativamente o desempenho das operações de escrita.

   3 - Cache otimizado: CQRS permite que você implemente caches de leitura para armazenar os resultados de consultas frequentes. Com a separação entre a camada de leitura e escrita, você pode facilmente manter e atualizar esses caches de forma independente, garantindo que as consultas sejam atendidas de forma mais rápida. Isso reduz a carga no sistema de banco de dados e melhora a velocidade de resposta das operações de leitura.

   4 - Redução de bloqueios e concorrência: Com a separação de responsabilidades entre comandos e consultas, você pode evitar bloqueios desnecessários no banco de dados. As operações de leitura podem ser executadas sem interferir nas operações de escrita, permitindo um melhor desempenho em cenários de concorrência. Isso é especialmente relevante em sistemas de alta carga, onde a performance é crítica.

   5 - Adoção de tecnologias específicas: Ao implementar CQRS, você pode escolher tecnologias específicas para cada parte do sistema, levando em consideração os requisitos de performance. Por exemplo, você pode optar por um banco de dados relacional otimizado para escrita e um mecanismo de indexação de alto desempenho para leitura. Isso permite a utilização das melhores ferramentas para cada contexto, maximizando a performance global do sistema.

# Mediator
O padrão Mediator é útil quando há muitas interações complexas entre objetos em um sistema e as dependências diretas entre esses objetos podem se tornar confusas e difíceis de manter. Ao introduzir o Mediator, cada objeto pode se concentrar em suas próprias responsabilidades, e a lógica de comunicação e coordenação é transferida para o objeto Mediator.

Vantagens do padrão Mediator:

Redução do acoplamento: O padrão Mediator ajuda a reduzir o acoplamento entre objetos, pois eles não precisam saber sobre a existência ou detalhes de outros objetos além do Mediator.

Facilita a manutenção: A lógica de comunicação está centralizada no Mediator, o que torna mais fácil modificar ou estender as interações entre objetos sem afetar suas implementações individuais.

Melhora a legibilidade: O padrão Mediator pode tornar o código mais legível, pois separa as responsabilidades de comunicação dos objetos em uma classe dedicada (o Mediator).

# SOLID
Os benefícios de aplicar os princípios SOLID no design de software são diversos e impactam positivamente a qualidade, manutenção e extensibilidade do código. Aqui estão os principais benefícios:

* Flexibilidade e extensibilidade: Ao seguir o Princípio do Aberto/Fechado (OCP), o código torna-se mais flexível para adicionar novas funcionalidades sem alterar o código existente. Isso facilita a extensão do sistema de forma modular, reduzindo o risco de introduzir bugs em partes já testadas e funcionais.

* Facilidade de manutenção: O Princípio da Responsabilidade Única (SRP) ajuda a garantir que cada classe tenha apenas uma única responsabilidade. Isso torna o código mais fácil de entender e alterar, pois as mudanças em uma funcionalidade específica podem ser feitas sem afetar outras partes do sistema.

* Reutilização de código: A aplicação do Princípio da Segregação de Interface (ISP) permite a criação de interfaces mais específicas, resultando em classes mais coesas. Isso possibilita a reutilização dessas classes em diferentes contextos, já que as interfaces são mais focadas e menos dependentes de detalhes de implementação.

* Maior coesão e menor acoplamento: Os princípios SOLID, em conjunto, promovem uma maior coesão dentro das classes e módulos do sistema. Com o Princípio da Inversão de Dependência (DIP), o acoplamento é reduzido, tornando o código mais independente e menos propenso a efeitos colaterais indesejados.

* Facilita testes unitários: O design orientado a objetos consistente, com uma única responsabilidade bem definida para cada classe (SRP) e interfaces específicas (ISP), torna mais fácil criar testes unitários focados e robustos.

* Facilita colaboração: Com código mais organizado e claro, os desenvolvedores podem colaborar mais facilmente. Além disso, a adoção de padrões de design consistentes torna o trabalho em equipe mais fluido, pois todos seguem as mesmas diretrizes.

* Redução de problemas de manutenção: Ao aplicar o Princípio da Substituição de Liskov (LSP), evitam-se problemas de incompatibilidade e comportamentos inesperados ao substituir classes derivadas. Isso contribui para a estabilidade do sistema.

* Melhoria na escalabilidade: Com um código mais estruturado e extensível, o sistema está melhor preparado para se adaptar a novas demandas e crescer ao longo do tempo.

    
