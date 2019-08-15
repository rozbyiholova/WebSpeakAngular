export class Category {
    public Id: number;
    public Name: string; 
    public Picture: string;
    public ParentId?: number;
}



/*
 *
 *  public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string Picture { get; set; }

        public virtual Categories Parent { get; set; }
        public virtual ICollection<CategoriesTranslations> CategoriesTranslations { get; set; }
        public virtual ICollection<Categories> InverseParent { get; set; }
        public virtual ICollection<TestResults> TestResults { get; set; }
        public virtual ICollection<Words> Words { get; set; }
 */