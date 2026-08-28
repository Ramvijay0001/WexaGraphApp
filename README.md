\# WexaGraph – CognoDB Graph Explorer



A full-stack graph exploration application built for the \*\*Wexa AI CognoDB Assignment\*\*.



WexaGraph allows users to explore relationships between technologies, projects, and business domains using \*\*Neo4j/CognoDB\*\* and an interactive \*\*Cytoscape.js\*\* graph visualization.



\---



\## 🚀 Features



\* Search technologies such as Angular, .NET, C#, TypeScript, and AI

\* Explore projects using a selected technology

\* Discover technology-to-domain relationships

\* Get related technology recommendations

\* Interactive graph visualization

\* Click graph nodes to view node details

\* Loading state

\* Empty state

\* Error handling

\* Database connectivity test endpoint

\* Database seed endpoint

\* Parameterized Cypher queries

\* Unit tests for API controller

\* Responsive Angular UI



\---



\## 🏗️ Architecture



```text

&#x20;                   ┌─────────────────────┐

&#x20;                   │      Angular UI     │

&#x20;                   │                     │

&#x20;                   │  Search / Results   │

&#x20;                   │  Cytoscape Graph    │

&#x20;                   └──────────┬──────────┘

&#x20;                              │ HTTP

&#x20;                              ▼

&#x20;                   ┌─────────────────────┐

&#x20;                   │   ASP.NET Core API  │

&#x20;                   │                     │

&#x20;                   │ DatabaseController  │

&#x20;                   │ CognoDbService      │

&#x20;                   │ SeedService         │

&#x20;                   └──────────┬──────────┘

&#x20;                              │

&#x20;                              │ Neo4j Driver

&#x20;                              ▼

&#x20;                   ┌─────────────────────┐

&#x20;                   │   CognoDB / Neo4j   │

&#x20;                   │                     │

&#x20;                   │ Technology          │

&#x20;                   │ Project             │

&#x20;                   │ Domain              │

&#x20;                   │ Relationships       │

&#x20;                   └─────────────────────┘

```



\---



\## 🧠 Why a Graph Database?



The application deals with relationships rather than simple tabular data.



For example:



```text

Angular

&#x20;  │

&#x20;  ├── RELATED\_TO ──> TypeScript

&#x20;  │

&#x20;  └── USED\_BY <──── Banking API Platform

&#x20;                          │

&#x20;                          └── IN\_DOMAIN ──> Banking

```



A relational database could represent these relationships using multiple tables and JOIN operations.



As the number of technologies, projects, domains, and relationships grows, relationship traversal becomes increasingly complex.



A graph database makes these relationships first-class entities.



For this use case, CognoDB/Neo4j provides advantages such as:



\* Natural representation of relationships

\* Easy multi-hop traversal

\* Flexible graph structure

\* Efficient relationship-oriented queries

\* Simple exploration of connected entities

\* Easier extension when new node or relationship types are introduced



\---



\## 📊 Data Model



\### Nodes



The application currently uses the following node types:



| Node       | Properties            | Description                   |

| ---------- | --------------------- | ----------------------------- |

| Technology | `name`, `category`    | Represents a technology       |

| Project    | `name`, `description` | Represents a software project |

| Domain     | `name`                | Represents a business domain  |



\### Relationships



| Relationship | From       | To         | Description                 |

| ------------ | ---------- | ---------- | --------------------------- |

| `RELATED\_TO` | Technology | Technology | Technology relationship     |

| `USES`       | Project    | Technology | Project uses a technology   |

| `IN\_DOMAIN`  | Project    | Domain     | Project belongs to a domain |



\### Example Graph



```text

&#x20;             ┌──────────────┐

&#x20;             │  TypeScript  │

&#x20;             └──────▲───────┘

&#x20;                    │

&#x20;               RELATED\_TO

&#x20;                    │

&#x20;             ┌──────┴───────┐

&#x20;             │    Angular   │

&#x20;             └──────▲───────┘

&#x20;                    │

&#x20;                   USES

&#x20;                    │

&#x20;             ┌──────┴──────────────┐

&#x20;             │ Banking API Platform│

&#x20;             └──────────┬──────────┘

&#x20;                        │

&#x20;                     IN\_DOMAIN

&#x20;                        │

&#x20;                 ┌──────▼──────┐

&#x20;                 │   Banking   │

&#x20;                 └─────────────┘

```



\---



\## 🗄️ Seed Data



The application includes a seed service that creates realistic graph data.



Example technologies:



\* Angular

\* .NET

\* TypeScript

\* C#

\* Artificial Intelligence



Example domains:



\* Banking

\* Healthcare



Example projects:



\* Banking API Platform

\* Healthcare Portal



The seed operation uses `MERGE`, making it safe to execute multiple times without unnecessarily creating duplicate nodes.



\---



\## 🔍 Main Graph Queries



\### 1. Projects by Technology



Find projects that use a particular technology.



Example:



```cypher

MATCH (p:Project)-\[:USES]->(t:Technology)

WHERE toLower(t.name) = toLower($technology)

RETURN p.name AS projectName

ORDER BY p.name

```



\---



\### 2. Technology Domains



Find projects using a technology and the business domain they belong to.



```cypher

MATCH (p:Project)-\[:USES]->(t:Technology)

MATCH (p)-\[:IN\_DOMAIN]->(d:Domain)

WHERE toLower(t.name) = toLower($technology)

RETURN

&#x20;   p.name AS projectName,

&#x20;   d.name AS domainName

ORDER BY p.name

```



\---



\### 3. Related Technology Recommendations



Find technologies related to the requested technology.



```cypher

MATCH (t:Technology)-\[:RELATED\_TO]->(related:Technology)

WHERE toLower(t.name) = toLower($technology)

RETURN

&#x20;   related.name AS technologyName,

&#x20;   related.category AS category

ORDER BY related.name

```



\---



\### 4. Multi-Hop Graph Query



The graph endpoint demonstrates multi-hop traversal across projects, technologies, related technologies, and domains.



Conceptually:



```text

Technology

&#x20;   │

&#x20;   ├── RELATED\_TO ──> Technology

&#x20;   │

&#x20;   └── USED\_BY <──── Project

&#x20;                        │

&#x20;                        └── IN\_DOMAIN ──> Domain

```



This type of connected traversal is one of the areas where a graph database provides a natural data model.



\---



\## 🔐 Parameterized Queries



All user-provided technology values are passed to Cypher queries as parameters.



Example:



```csharp

var parameters = new

{

&#x20;   technology

};



await session.RunAsync(cypher, parameters);

```



The application does not construct Cypher by concatenating user input.



This helps prevent Cypher injection and keeps query construction clean.



\---



\## 🛠️ Technology Stack



\### Backend



\* .NET 8

\* ASP.NET Core Web API

\* C#

\* Neo4j Official .NET Driver

\* CognoDB

\* xUnit

\* Moq



\### Frontend



\* Angular

\* TypeScript

\* HTML

\* CSS

\* Cytoscape.js



\### Development



\* Visual Studio Code

\* PowerShell

\* Git

\* GitHub



\---



\## 📁 Project Structure



```text

WexaGraphApp/

│

├── WexaGraph.Api/

│   ├── Controllers/

│   │   └── DatabaseController.cs

│   │

│   ├── Services/

│   │   ├── CognoDbService.cs

│   │   ├── ICognoDbService.cs

│   │   └── SeedService.cs

│   │

│   └── Program.cs

│

├── WexaGraph.Api.Tests/

│   └── UnitTest1.cs

│

├── WexaAppui/

│   └── wexa-graph-ui/

│       └── src/

│           └── app/

│               ├── app.component.ts

│               ├── app.component.html

│               ├── app.component.css

│               │

│               └── graph-view/

│                   ├── graph-view.component.ts

│                   ├── graph-view.component.html

│                   └── graph-view.component.css

│

└── README.md

```



\---



\## ⚙️ Prerequisites



Install the following:



\* .NET 8 SDK

\* Node.js

\* npm

\* Angular CLI

\* CognoDB account / instance

\* Git



\---



\## ☁️ CognoDB Setup



Create a free CognoDB instance and obtain:



```text

Bolt URI

Username

Password

```



The application should read database credentials from environment/configuration values rather than committing secrets to Git.



Example:



```text

bolt+s://<instance-id>.databases.cognodb.cloud

```



Never commit the actual database password to the repository.



\---



\## 🔧 Backend Configuration



Configure the CognoDB connection using environment variables or local configuration.



Example environment variables:



```text

COGNODB\_URI=<your-cognodb-uri>

COGNODB\_USERNAME=cognodb

COGNODB\_PASSWORD=<your-password>

```



Do not commit real credentials.



\---



\## ▶️ Run the Backend



From the repository root:



```powershell

cd WexaGraph.Api

dotnet restore

dotnet build

dotnet run

```



The API will start on the configured ASP.NET Core URL.



Example:



```text

https://localhost:7219

```



\---



\## 🌱 Seed the Database



After starting the API, execute:



```http

POST /api/Database/seed

```



Example using PowerShell:



```powershell

Invoke-RestMethod `

&#x20; -Method Post `

&#x20; -Uri "https://localhost:7219/api/Database/seed"

```



The seed operation creates the sample technologies, projects, domains, and relationships.



\---



\## 🔌 Test Database Connection



Use:



```http

GET /api/Database/test

```



Example:



```powershell

Invoke-RestMethod `

&#x20; -Method Get `

&#x20; -Uri "https://localhost:7219/api/Database/test"

```



Expected response:



```json

{

&#x20; "success": true,

&#x20; "message": "CognoDB connection successful."

}

```



\---



\## 🌐 API Endpoints



\### Test Connection



```http

GET /api/Database/test

```



\### Seed Database



```http

POST /api/Database/seed

```



\### Projects by Technology



```http

GET /api/Database/projects-by-technology?technology=Angular

```



\### Technology Domains



```http

GET /api/Database/technology-domains?technology=Angular

```



\### Recommendations



```http

GET /api/Database/recommendations?technology=Angular

```



\### Graph



```http

GET /api/Database/graph?technology=Angular

```



\---



\## 🖥️ Run the Angular Application



Open another terminal:



```powershell

cd WexaAppui\\wexa-graph-ui

npm install

npm start

```



Or:



```powershell

ng serve

```



Open:



```text

http://localhost:4200

```



\---



\## 🔎 Example Usage



Enter:



```text

Angular

```



and click:



```text

Explore

```



The application displays:



\### Projects



```text

Banking API Platform

Healthcare Portal

```



\### Related Technologies



```text

TypeScript

```



\### Domains



```text

Banking API Platform → Banking

Healthcare Portal → Healthcare

```



The graph visualization then displays the connected nodes and relationships.



\---



\## 🕸️ Graph Visualization



The frontend uses \*\*Cytoscape.js\*\* to visualize the graph.



Node types include:



\* Technology

\* Related Technology

\* Project

\* Domain



Users can click a node to display its details.



Example:



```text

Selected Node



Name: Angular

Type: technology

```



\---



\## 🧪 Testing



Run all tests:



```powershell

dotnet test .\\WexaGraphApp.sln

```



Current test suite covers controller behavior using mocked services.



Example successful result:



```text

Test summary: total: 2

failed: 0

succeeded: 2

skipped: 0

```



\---



\## 🏗️ Production Build



\### Backend



```powershell

dotnet build .\\WexaGraphApp.sln

```



\### Angular



```powershell

cd WexaAppui\\wexa-graph-ui

npm run build

```



The Angular production output is generated under:



```text

dist/wexa-graph-ui

```



\---



\## 🖼️ Screenshots



\### Main Application



> Add screenshot here.



```text

docs/screenshots/main-screen.png

```



\### Graph Visualization



> Add screenshot here.



```text

docs/screenshots/graph-view.png

```



\### Node Selection



> Add screenshot here.



```text

docs/screenshots/node-selection.png

```



\---



\## 🌍 Hosted Demo



\*\*Live Application:\*\*



> Add deployed application URL here.



```text

https://<your-demo-url>

```



The hosted demo allows reviewers to interact with the application without setting up the project locally.



\---



\## 🎥 Screen Recording



\*\*Demo Video:\*\*



> Add screen recording link here.



```text

<your-video-link>

```



The recording demonstrates:



1\. Opening the application

2\. Searching for a technology

3\. Viewing projects

4\. Viewing domains

5\. Viewing recommendations

6\. Exploring the graph

7\. Selecting a graph node

8\. Showing the overall user experience



\---



\## 🔒 Security



Sensitive configuration should never be committed to Git.



Do not commit:



```text

Database passwords

API keys

Connection secrets

Environment-specific credentials

```



Use environment variables or secure deployment configuration instead.



\---



\## 🚨 Error Handling



The API handles database and service failures and returns appropriate HTTP responses.



For example:



```json

{

&#x20; "success": false,

&#x20; "message": "Unable to connect to CognoDB.",

&#x20; "error": "..."

}

```



The Angular application also provides user-friendly loading and error states.



\---



\## 📌 Design Decisions



\### Why ASP.NET Core?



ASP.NET Core provides:



\* Strong C# ecosystem

\* Dependency Injection

\* Async programming

\* Clean Web API architecture

\* Good testing support



\### Why Angular?



Angular provides:



\* Structured frontend architecture

\* TypeScript

\* Component-based UI

\* Strong HTTP client support

\* Good maintainability



\### Why Cytoscape.js?



Cytoscape.js is designed specifically for graph/network visualization and provides:



\* Interactive nodes

\* Directed edges

\* Graph layouts

\* Node events

\* Relationship visualization



\### Why CognoDB?



CognoDB provides a graph-oriented model that fits the application's core requirement of exploring relationships between technologies, projects, and domains.



\---



\## ✅ Assignment Requirements Checklist



| Requirement                     | Status                   |

| ------------------------------- | ------------------------ |

| Graph-based real-world use case | ✅                        |

| Thoughtful graph data model     | ✅                        |

| Labeled nodes                   | ✅                        |

| Typed relationships             | ✅                        |

| Node properties                 | ✅                        |

| Realistic seed data             | ✅                        |

| Seed script/service             | ✅                        |

| Cypher queries                  | ✅                        |

| Multi-hop traversal             | ✅                        |

| Parameterized queries           | ✅                        |

| Functional web application      | ✅                        |

| Loading state                   | ✅                        |

| Empty state                     | ✅                        |

| Error handling                  | ✅                        |

| Interactive graph visualization | ✅                        |

| Unit tests                      | ✅                        |

| README                          | ✅                        |

| Data model documentation        | ✅                        |

| Setup instructions              | ✅                        |

| Screenshots                     | 🔄 Add before submission |

| Hosted demo                     | 🔄 Add before submission |

| Screen recording                | 🔄 Add before submission |



\---



\## 📮 Submission



Repository:



```text

https://github.com/Ramvijay0001/WexaGraphApp

```



Assignment submission email:



```text

hr@wexa.ai

```



Subject:



```text

CognoDB Assignment 2 – Ramvijay

```



\---



\## 👨‍💻 Author



\*\*Ramvijay\*\*



Software Engineer



GitHub:



https://github.com/Ramvijay0001



\---



\## 📄 Assignment



This project was created as part of the Wexa AI CognoDB Candidate Take-Home Assignment.



