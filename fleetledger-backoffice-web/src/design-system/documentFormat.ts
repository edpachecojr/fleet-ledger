/**
 * Máscara de exibição do CNPJ guardado só com dígitos; outro tamanho volta como veio.
 * @example formatCnpj('11222333000181') // '11.222.333/0001-81'
 */
export function formatCnpj(digits: string | null | undefined): string {
  const value = digits ?? ''
  return /^\d{14}$/.test(value)
    ? value.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})$/, '$1.$2.$3/$4-$5')
    : value
}

/**
 * Máscara de exibição do CPF guardado só com dígitos.
 * @example formatCpf('52998224725') // '529.982.247-25'
 */
export function formatCpf(digits: string | null | undefined): string {
  const value = digits ?? ''
  return /^\d{11}$/.test(value) ? value.replace(/^(\d{3})(\d{3})(\d{3})(\d{2})$/, '$1.$2.$3-$4') : value
}

/**
 * Telefone com DDD: celular (11 dígitos) ou fixo (10).
 * @example formatPhone('11987654321') // '(11) 98765-4321'
 */
export function formatPhone(digits: string | null | undefined): string {
  const value = digits ?? ''
  if (/^\d{11}$/.test(value)) return value.replace(/^(\d{2})(\d{5})(\d{4})$/, '($1) $2-$3')
  return /^\d{10}$/.test(value) ? value.replace(/^(\d{2})(\d{4})(\d{4})$/, '($1) $2-$3') : value
}
