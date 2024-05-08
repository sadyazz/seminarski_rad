import {KorisnickiNalog} from "./korisnickiNalog";


export interface AutentifikacijaToken{
  id: number
  vrijednost: string
  korisnickiNalogId: number
  korisnickiNalog: KorisnickiNalog
  vrijemeEvidentiranja: string
  ipAdresa: string
}
