"use client"

import { useState, useEffect } from "react"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Button } from "@/components/ui/button"
import type { Customer } from "@/lib/api-server"

interface CustomerFilterProps {
  customers: Customer[]
}

export function CustomerFilter({ customers }: CustomerFilterProps) {
  const [selectedCustomerId, setSelectedCustomerId] = useState<string>("")

  useEffect(() => {
    const rows = document.querySelectorAll(".customer-row") as NodeListOf<HTMLElement>

    if (!selectedCustomerId) {
      rows.forEach((row) => {
        row.style.display = ""
      })
      return
    }

    rows.forEach((row) => {
      const customerId = row.dataset.customerId
      if (customerId === selectedCustomerId) {
        row.style.display = ""
      } else {
        row.style.display = "none"
      }
    })
  }, [selectedCustomerId])

  return (
    <div className="flex items-center gap-4">
      <Label htmlFor="customerFilter" className="text-sm font-medium">
        Filtrar por cliente:
      </Label>
      <Select value={selectedCustomerId} onValueChange={setSelectedCustomerId}>
        <SelectTrigger id="customerFilter" className="w-[250px]">
          <SelectValue placeholder="Todos os clientes" />
        </SelectTrigger>
        <SelectContent>
          <SelectItem value="all">Todos os clientes</SelectItem>
          {customers.map((customer) => (
            <SelectItem key={customer.id} value={customer.id}>
              {customer.name}
            </SelectItem>
          ))}
        </SelectContent>
      </Select>
      {selectedCustomerId && (
        <Button variant="ghost" size="sm" onClick={() => setSelectedCustomerId("")}>
          Limpar filtro
        </Button>
      )}
    </div>
  )
}
