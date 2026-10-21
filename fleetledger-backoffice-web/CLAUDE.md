# CLAUDE.md — fleetledger-backoffice-web

Backoffice web do FleetLedger. Complementa o `CLAUDE.md` da raiz, que continua
valendo integralmente (fluxo de trabalho, `docs/PROGRESS.md`, ADRs, commits).

SPA em React que fala com a API **apenas pelo contrato HTTP/OpenAPI** de
`fleetledger-api`. Nenhuma regra de negócio mora aqui: validação, autorização e
cálculo são do servidor; validação no cliente é conveniência de UX.

| Camada            | Escolha                                                     | Motivo                                                               |
| ----------------- | ----------------------------------------------------------- | -------------------------------------------------------------------- |
| UI                | React 19 + TypeScript 6 (strict)                            | TS 7 ainda não é suportado pelo typescript-eslint/openapi-typescript |
| Build             | Vite 8 (SPA estática, servida por nginx)                    | `docs/adr/0006`                                                      |
| Rotas             | React Router 8 (modo biblioteca)                            | `docs/adr/0006`                                                      |
| Dados do servidor | TanStack Query 5                                            | cache, refetch, estados de carregamento                              |
| Cliente HTTP      | `openapi-fetch` + tipos gerados por `openapi-typescript`    | contrato único com a API                                             |
| Estado local      | Zustand (só preferências de tema)                           |                                                                      |
| Estilo            | Tailwind CSS 4 + tokens do design system **Backoffice**     |                                                                      |
| Ícones            | `lucide-react`                                              | regra do design system                                               |
| Testes            | Vitest + Testing Library + jsdom                            |                                                                      |
| Pacotes           | pnpm, versões exatas, `minimumReleaseAge` padrão respeitado |                                                                      |

## Comandos

```bash
pnpm install
pnpm dev              # http://localhost:5173 — /api vai para http://localhost:5080
pnpm test             # toda a suíte, sem rede nem API no ar
pnpm lint             # ESLint (inclui as regras de tamanho abaixo)
pnpm typecheck
pnpm format           # Prettier decide o estilo; não discuta além disso
pnpm build
pnpm api:types        # regenera src/api/generated/schema.d.ts (API no ar)
```

**Antes de declarar concluído:** `pnpm typecheck && pnpm lint && pnpm format:check && pnpm test && pnpm build`
verdes, este arquivo ainda verdadeiro e `docs/PROGRESS.md` atualizado.

## Estrutura

```
src/
├── main.tsx                 composição: cria cliente HTTP, query client e router
├── index.css                Tailwind + tokens + base
├── app/                     rotas (appRoutes), providers, constantes de app
├── api/                     cliente HTTP, injeção (ApiClientContext), erros, recusas de
│                            escrita (mutationOutcome), formulário de cadastro
│                            (useMutationForm), filtros de listagem (listQuery)
│   └── generated/           schema.d.ts GERADO — nunca editar à mão
├── design-system/           tokens, tema, componentes genéricos (sem domínio)
│   ├── components/          Button, Card, Badge, DataTable, TextField, SelectField,
│   │                        ConfirmDialog, TablePagination, PageHeader, EmptyState…
│   ├── numberFormat.ts      moeda, quantidades e leitura de número em pt-BR
│   ├── settings/            painel de aparência (modo, cor, densidade, direção)
│   ├── styles/tokens.css    valores do design system Backoffice
│   └── theme/               store, applyTheme
├── layout/                  AppShell, Sidebar, Header, LogoutButton, navigation.ts
├── session/                 AdminSession, SessionProvider, RequireSession (proteção de rotas)
├── features/<funcionalidade>/   páginas, hooks, chamadas e componentes do domínio
└── test/                    setup, renderApp, fakes/
```

| Artefato             | Caminho                                                                  |
| -------------------- | ------------------------------------------------------------------------ |
| Página               | `features/<funcionalidade>/<Nome>Page.tsx` + rota em `app/appRoutes.tsx` |
| Chamada à API        | `features/<funcionalidade>/fetch<Recurso>.ts` (recebe `ApiClient`)       |
| Hook de dados        | `features/<funcionalidade>/use<Recurso>.ts` (TanStack Query)             |
| Mapeamento para tela | `features/<funcionalidade>/<recurso>.ts` (tipos, rótulos, guards)        |
| Item de menu         | `layout/navigation.ts`                                                   |
| Componente genérico  | `design-system/components/<Nome>.tsx`                                    |
| Teste                | ao lado do arquivo: `<arquivo>.test.ts(x)`                               |
| Fake                 | `test/fakes/Fake<Coisa>.ts`                                              |

`design-system/` não importa de `features/`; `features/` não importa de outra
feature (o que for comum sobe para `design-system/` ou `api/`).

## Princípios (verificados no review; os marcados com ⚙ também pelo ESLint)

1. **Funções e arquivos pequenos.** Função faz UMA coisa, bem, e só ela: 4–20
   linhas, máximo 30 ⚙. Arquivo idealmente < 200 linhas, máximo 300 ⚙.
   Componente que cresce vira componentes menores no mesmo arquivo ou em outro.
2. **SRP.** Um módulo, uma razão para mudar. Componente não busca dado _e_
   formata _e_ desenha: chamada em `fetch*.ts`, cache em `use*.ts`, mapeamento em
   `<recurso>.ts`, desenho no `.tsx`.
3. **Nomes significativos e únicos.** Revelam intenção, sem desinformação,
   pronunciáveis e pesquisáveis. Proibidos como nome inteiro: `data`, `info`,
   `handler`, `manager`, `helper`, `utils`, `item`, `temp`. Um bom nome retorna
   menos de 5 resultados num `grep`. Arquivo tem o nome do que exporta.
4. **Comentários com contexto e proveniência.** Escreva o PORQUÊ, nunca o O QUÊ;
   cite issue/ADR/commit quando a linha existe por causa deles. Preserve
   comentários existentes ao refatorar (corrija se ficaram errados). JSDoc com
   intenção + um `@example` nas funções exportadas que não são óbvias.
5. **Tipos explícitos.** `strict`, `noUncheckedIndexedAccess`,
   `exactOptionalPropertyTypes`. Exportações com tipo de retorno explícito ⚙. Sem
   `any` ⚙, sem `as` para calar o compilador (só com comentário do porquê). Tipos
   da API vêm de `src/api/generated/schema.d.ts`, nunca escritos à mão.
6. **DRY.** Na terceira repetição, extraia (função, componente, hook). Mas
   cuidado com o falso DRY: o que muda por razões diferentes fica separado.
7. **Testes F.I.R.S.T.** Rápidos, independentes, repetíveis, autoverificáveis e
   escritos junto com o código. Toda função nova ganha teste; correção de bug
   ganha teste que falha antes.
8. **Estrutura previsível.** Siga a tabela acima. Arquivo fora do lugar é defeito.
9. **Injeção de dependência.** I/O entra por parâmetro, props ou contexto, nunca
   importado direto num componente: `ApiClient` via `ApiClientProvider`,
   `fetch` via `createApiClient({ fetch })`, ambiente do tema via
   `ThemeEnvironment`. Por isso os testes trocam tudo por fakes.
10. **Sem aninhamento profundo.** Máximo 2 níveis ⚙, retorno antecipado, um nível
    de abstração por função. No JSX, condição complexa vira componente.
11. **Erro com contexto.** Mensagem diz o que se tentou, com qual valor e o que se
    esperava (`ApiRequestError`: método, rota, status recebido, formato
    esperado). Nunca engula exceção; nunca `catch {}` vazio.
12. **Sem comentários óbvios.** `// renderiza o botão` é ruído: apague.

Também: no máximo 3 parâmetros por função ⚙ (agrupe num objeto); sem
`console.*` ⚙; sem `Date.now()`/`new Date()` solto em regra (injete o relógio).

## Design system Backoffice

Fonte: https://claude.ai/artifact/6Hc6H2JrFv8vKJHRoM5LA9 (README, Implementation e
cards de componente). O guia foi escrito para Next.js; aqui ele é aplicado a uma
SPA Vite, com os mesmos tokens e as mesmas receitas Tailwind.

- **Use tokens pelo nome** (`bg-surface`, `text-text-secondary`,
  `shadow-[var(--shadow-card)]`). Nunca hex/rgb solto em componente.
- **Uma cor de destaque** (`primary`) aponta a ação; cores de status só
  significam status, e o Badge sempre carrega o texto.
- **Card é o único container**: `surface`, raio 16px, sombra, sem borda, 24px
  de padding, 24px entre cards. Nunca card dentro de card.
- **Densidade**: paddings e alturas de linha multiplicam `var(--density)`.
  **Direção**: use propriedades lógicas (`ps-*`, `pe-*`, `start-*`, `end-*`).
- Ícones só `lucide-react`, stroke 2; sem emoji.
- Texto em pt-BR, frase com inicial maiúscula, botões são verbos ("Salvar").
  Moeda `R$ 1.249,00`; números em `tabular-nums`.
- Componente novo do design system: siga o card correspondente do artefato e
  coloque em `design-system/components/`.

## Integração com a API

- O navegador chama sempre `/api/...` (mesma origem). Em dev o Vite encaminha
  para `API_PROXY_TARGET` (padrão `http://localhost:5080`); no compose, o nginx
  encaminha para `API_UPSTREAM`. Por isso a API não precisa de CORS.
- Mudou a API? Suba a API e rode `pnpm api:types`; o TypeScript aponta cada uso
  que quebrou. O arquivo gerado é versionado.
- Autenticação (`docs/adr/0007`): `AdminSession` (`src/session/`) é o
  `AccessTokenSource` do `ApiClient`, guarda o token em `sessionStorage` e encerra a
  sessão no `expiresAt` ou quando a API responde 401 a uma requisição com token.
  Toda rota fica dentro de `RequireSession`, exceto `/login`.
- Respostas de erro com corpo útil (ex.: `/health` 503) são dado, não exceção;
  resposta fora do contrato vira `ApiRequestError`.

## Testes

- Nome em português ou inglês, descrevendo o comportamento:
  `it('shows the database as unavailable when the API answers 503')`.
- Arrange / Act / Assert separados por linha em branco.
- I/O vira **fake nomeado** em `src/test/fakes/` (`FakeHttpBackend`,
  `FakeMatchMedia`), nunca stub inline de `fetch`. `vi.fn()` só para verificar
  interação (ex.: `onClose` chamado).
- Fluxo de tela: `renderApp(backend, rota)` monta a aplicação real em memória, já
  logada; passe `signedOutSession().session` (`src/test/testSessions.ts`) como terceiro
  argumento para testar sem sessão.
- Consulte a tela como o usuário: por papel e nome acessível (`getByRole`).
