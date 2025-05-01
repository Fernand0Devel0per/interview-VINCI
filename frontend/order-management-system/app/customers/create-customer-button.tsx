"use client"

import { useState } from "react"
import { Button } from "@/components/ui/button"
import { Plus } from "lucide-react"
import { CreateCustomerDialog } from "./create-customer-dialog"

export function CreateCustomerButton() {
  const [isDialogOpen, setIsDialogOpen] = useState(false)

  return (
    <>
      <Button onClick={() => setIsDialogOpen(true)}>
        <Plus className="mr-2 h-4 w-4" />
        Adicionar Cliente
      </Button>

      <CreateCustomerDialog open={isDialogOpen} onOpenChange={setIsDialogOpen} />
    </>
  )
}
