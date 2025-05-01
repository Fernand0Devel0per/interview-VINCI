import Link from "next/link"
import { Button } from "@/components/ui/button"

export default function NotFound() {
  return (
    <div className="flex flex-col items-center justify-center min-h-[70vh] text-center">
      <h1 className="text-6xl font-bold mb-4">404</h1>
      <h2 className="text-2xl font-semibold mb-6">Página não encontrada</h2>
      <p className="text-muted-foreground mb-8">A página que você está procurando não existe ou foi removida.</p>
      <Link href="/">
        <Button>Voltar para o Dashboard</Button>
      </Link>
    </div>
  )
}
