"use client"

import { useState } from "react"
import { Button } from "@/components/ui/button"
import { Pencil, Trash2 } from "lucide-react"
import { EditCustomerDialog } from "./edit-customer-dialog"
import { deleteCustomer } from "@/lib/api-server"
import { useToast } from "@/components/ui/use-toast"
import type { Customer } from "@/lib/api-server"

interface CustomerActionsProps {
  customer: Customer
}

export function CustomerActions({ customer }: CustomerActionsProps) {
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false)
  const { toast } = useToast()

  const handleDelete = async () => {
    if (!confirm("Tem certeza que deseja excluir este cliente?")) return

    try {
      await deleteCustomer(customer.id)
      toast({
        title: "Sucesso",
        description: "Cliente excluído com sucesso",
      })
    } catch (error) {
      toast({
        title: "Erro",
        description: error instanceof Error ? error.message : "Falha ao excluir cliente",
        variant: "destructive",
      })
    }
  }

  return (
    <>
      <Button variant="ghost" size="icon" onClick={() => setIsEditDialogOpen(true)}>
        <Pencil className="h-4 w-4" />
      </Button>
      <Button variant="ghost" size="icon" onClick={handleDelete}>
        <Trash2 className="h-4 w-4" />
      </Button>

      <EditCustomerDialog customer={customer} open={isEditDialogOpen} onOpenChange={setIsEditDialogOpen} />
    </>
  )
}
