# QuarentenarioWeb

Projeto Web do Sistema de Gestão de dados do Laboratório de Biologia Molecular do Quarentenário [IAC - Instituto Agronômico de Campinas](https://www.iac.sp.gov.br) desenvolvido como parte do Projeto Integrador da UNIVESP. No repositório [QuarentenarioWDB](https://github.com/24227051/QuarentenarioDB) se encontra o banco de dados que também faz parte desse projeto

### Disciplina
Projeto Integrador em Computação I. Tecnologia da Informação. UNIVESP - Universidade Virtual do Estado de São Paulo

## Motivação
O Projeto Integrador trata de resolução de problemas e os estudantes devem ser expostos a atividades que visam relacionar conteúdos curriculares a práticas profissionais. Cada Projeto Integrador é estruturado para consolidar o conhecimento das disciplinas dos semestres anteriores em atividades práticas que visam buscar a solução para um problema e sua implementação. O objetivo é se chegar a um protótipo funcional que sirva como prova de conceito das ideias propostas. 

## Definição
Gerenciador de amostras no meio bioquímico através de uma interface web com persistência em banco de dados e controle de versão do código-fonte 

### Características
- Gerenciador de Análises
- Gerenciador de Anexos
- Gerenciador de Boletins
- Gerenciados de Materiais
- Gerenciador de Materiais x Patógenos
- Gerenciador de Patógenos
- Gerenciador de Países
- Gerenciador de Tipos Controle
- Gerenciador de Tipos Patógeno

## Requisitos
.NET 10

## Primeiros passos
1. Baixar e instalar o [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/) que é uma IDE gratuita e completa para estudantes, projetos de código aberto e desenvolvedores individuais..
2. Baixar o projeto clicando no botão verde Code. Por exemplo, baixe o ZIP e instale em um diretório C:\dev
3. Abrir o projeto clicando no arquivo QuarentenarioWeb.slnx e rodar usando http
4. Criar um usuário. Abrir o SQL Server Management Studio (SSMS). selecionar o Id da tabela AspNetUsers e executar a procedure sp_incluir_funcoes passando o Id
