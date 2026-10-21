import type { Customer, CustomerPage } from '@/features/customers/customer'

export const CUSTOMER_ID = '01a15f55-beea-7bbe-9864-03350c62588b'

export function customerFixture(overrides: Partial<Customer> = {}): Customer {
  return {
    id: CUSTOMER_ID,
    corporateName: 'Agro Sul Ltda',
    tradeName: 'Agro Sul',
    cnpj: '11222333000181',
    phone: '11987654321',
    email: 'financeiro@agrosul.com.br',
    billingCycle: 'Monthly',
    paymentTermDays: 30,
    isActive: true,
    deactivatedAt: null,
    createdAt: '2026-10-21T12:00:00Z',
    updatedAt: '2026-10-21T12:00:00Z',
    ...overrides,
  }
}

export function customerPageBody(customers: Customer[]): CustomerPage {
  return {
    data: customers,
    totalCount: customers.length,
    currentPage: 1,
    pageSize: 25,
    totalPages: 1,
    code: 200,
  }
}

export function customerBody(
  customer: Customer,
  code = 200,
): { data: Customer; code: number; message: null } {
  return { data: customer, code, message: null }
}
