# ADR 0006 — Front-end: SPA React com Vite

**Data:** 2026-10-20 · **Status:** aceito

## Contexto
O backoffice é usado só pelo administrador autenticado e consome a API .NET pelo contrato OpenAPI. O design system adotado (Backoffice) traz um guia de implementação escrito para Next.js.

## Decisão
- **React 19 + Vite 8**, SPA estática, em `fleetledger-backoffice-web/`. Em produção, nginx serve os arquivos e encaminha `/api/*` para a API (mesma origem, sem CORS). Em desenvolvimento, o proxy do Vite faz o mesmo.
- **React Router 8** (modo biblioteca), **TanStack Query 5** para dados do servidor, **Zustand** só para preferências de tema.
- **Cliente HTTP gerado do contrato**: `openapi-typescript` gera os tipos a partir de `/openapi/v1.json`; `openapi-fetch` faz as chamadas. Tipos da API nunca são escritos à mão.
- **Design system Backoffice** aplicado com Tailwind CSS 4: tokens copiados do guia, fontes Outfit/JetBrains Mono via `@fontsource` (sem CDN), script de boot do tema no `index.html`.
- **TypeScript 6.0**: o 7.0 ainda não é aceito pelo typescript-eslint nem pelo openapi-typescript.
- **pnpm** com versões exatas e a política `minimumReleaseAge` padrão (sem exceções).
- O `/health` da API virou endpoint Minimal API documentado no OpenAPI, para que o front-end receba o tipo da resposta.

## Alternativas consideradas
- **Next.js (App Router)**: segue o guia do design system ao pé da letra, mas adiciona SSR e um servidor Node sem benefício para uma tela autenticada que só consome a API.
- **Cliente HTTP escrito à mão**: diverge do contrato em silêncio; o gerado quebra a compilação quando a API muda.

## Consequências
- Recarregar a página em qualquer rota exige o fallback para `index.html` (configurado no nginx).
- Ajustes do guia do DS que dependem de Next.js (`next/font`, `layout.tsx`) foram traduzidos; os tokens e as receitas Tailwind são os mesmos.
