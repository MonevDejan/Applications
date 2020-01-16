import { EventEmitter, Injectable } from '@angular/core';

import { Recipe } from './recipe.model';
import { Ingredient } from '../shared/ingredient.mode';
import { ShoppingListService } from '../shopping-list/shopping-list.service';

@Injectable()
export class RecipeService {
    private recipes: Recipe[] = [
        new Recipe(
            'Tasty Schnitzel', 
            'Awesome and delicious Schnitzel ', 
            'https://www.daringgourmet.com/wp-content/uploads/2014/03/Schnitzel-5.jpg', 
            [
                new Ingredient('Meat', 1),
                new Ingredient('French fries', 20),
            ]
        ),
        new Recipe(
            'Big fat burger', 
            'The fatest burger ever made!', 
            'https://assets3.thrillist.com/v1/image/2806345/size/tmg-article_default_mobile.jpg', 
            [
                new Ingredient('Buns', 2),
                new Ingredient('Meat', 2),
                new Ingredient('Cheese', 4),
            ]
        ),
    ];

    recipeSelected = new EventEmitter<Recipe>();
    
    constructor(private shoppingListService: ShoppingListService) {}
    getRecipes(){
    return this.recipes.slice();
    }

    addRecipe(recipe: Recipe){

    }

    addIngredientsToShoppingList(ingredients: Ingredient[]){
        this.shoppingListService.addIngredients(ingredients);
    }
}