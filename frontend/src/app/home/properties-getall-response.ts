

export interface PropertiesGetAllResponse {
  id: number
  userID: number
  user: User
  name: string
  adress: string
  numberOfRooms: number
  numberOfBathrooms: number
  pricePerNight: number
  cityID: number
  city: City
  propertyTypeID: number
  propertyType: PropertyType
  propertiesAmenities: PropertiesAmenity[]
  wishlist: Wishlist[]
  images: PropertyImage[];
  latitude:number,
  longitude:number
}

export interface CoordinateDto {
  latitude: number;
  longitude: number;
}

export interface PropertyImage {
  id: number;
  path: string;
}

export interface User {
  username: string
  isAdministrator: boolean
  isKorisnik: boolean
  id: number
  name: string
  surname: string
  email: string
  phone: string
  dateOfRegistraion: string
  dateBirth: string
  wishlist: Wishlist[]
}

export interface Wishlist {
  userID: number
  user: string
  propertiesID: number
  properties: string
  dateAdd: string
}

export interface City {
  id: number
  name: string
  countryID: number
  country: Country
}

export interface Country {
  id: number
  name: string
}

export interface PropertyType {
  id: number
  name: string
  description: string
}

export interface PropertiesAmenity {
  propertiesID: number
  properties: string
  amenitiesID: number
  amenities: Amenities
}

export interface Amenities {
  id: number
  name: string
  description: string
  propertiesAmenities: string[]
}
