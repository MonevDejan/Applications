import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Ingredient } from 'src/app/shared/ingredient.mode';
import { ShoppingListService } from '../shopping-list.service';

@Component({
  selector: 'app-shopping-edit',
  templateUrl: './shopping-edit.component.html',
  styleUrls: ['./shopping-edit.component.css']
})
export class ShoppingEditComponent implements OnInit {

  @ViewChild('inputName', {static: false})  nameInputRef : ElementRef;
  @ViewChild('inputAmount', {static: false}) amountInputRef : ElementRef;
  
  constructor(private shoppingListServise: ShoppingListService) { }

  ngOnInit() {
  }
 
  onAddIngredient(){
    const ingrediantName = this.nameInputRef.nativeElement.value;
    const ingrediantAmount = this.amountInputRef.nativeElement.value;
    const newIngrediet = new Ingredient(ingrediantName,ingrediantAmount);

    this.shoppingListServise.addIngredient(newIngrediet);
  }

}
