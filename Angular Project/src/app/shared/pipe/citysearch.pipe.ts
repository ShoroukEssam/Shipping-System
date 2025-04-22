import { Pipe, PipeTransform } from '@angular/core';
import { city } from '../Models/city';


@Pipe({
  name: 'citysearch',
  standalone: true,
})
export class CitySearchPipe implements PipeTransform {
  transform(cities: city[], term: string): city[] {
    return cities.filter((city) =>
      city.name.toLowerCase().includes(term.toLowerCase())
    );
  }
}
