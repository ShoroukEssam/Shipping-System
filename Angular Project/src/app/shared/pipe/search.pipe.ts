import { getAllBranch } from '../Models/branch';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'search',
  standalone: true,
})
export class SearchPipe implements PipeTransform {
  transform(products: getAllBranch[], term: string): getAllBranch[] {
    return products.filter((product) =>
      product.name.toLowerCase().includes(term.toLowerCase())
    );
  }
}
