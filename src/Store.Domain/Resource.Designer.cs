﻿//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Store.Domain {
    using System;
    
    
    /// <summary>
    ///   Uma classe de recurso de tipo de alta segurança, para pesquisar cadeias de caracteres localizadas etc.
    /// </summary>
    // Essa classe foi gerada automaticamente pela classe StronglyTypedResourceBuilder
    // através de uma ferramenta como ResGen ou Visual Studio.
    // Para adicionar ou remover um associado, edite o arquivo .ResX e execute ResGen novamente
    // com a opção /str, ou recrie o projeto do VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Retorna a instância de ResourceManager armazenada em cache usada por essa classe.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Store.Domain.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Substitui a propriedade CurrentUICulture do thread atual para todas as
        ///   pesquisas de recursos que usam essa classe de recurso de tipo de alta segurança.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Request Body Content: {Body}.
        /// </summary>
        public static string msgContentBody {
            get {
                return ResourceManager.GetString("msgContentBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Deleting order with number {Number}.
        /// </summary>
        public static string msgDeletingOrder {
            get {
                return ResourceManager.GetString("msgDeletingOrder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Error retrieving paged orders - Page {PageNumber}, Size {PageSize}.
        /// </summary>
        public static string msgErrorOrderPage {
            get {
                return ResourceManager.GetString("msgErrorOrderPage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Attempt to sell more than 20 identical items for Order Item Id {0}.
        /// </summary>
        public static string msgItemsMoreThan20 {
            get {
                return ResourceManager.GetString("msgItemsMoreThan20", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Order number {0} already exists..
        /// </summary>
        public static string msgOrderAlreadyExists {
            get {
                return ResourceManager.GetString("msgOrderAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Error order: {0}.
        /// </summary>
        public static string msgOrderError {
            get {
                return ResourceManager.GetString("msgOrderError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Order Is Cancelled..
        /// </summary>
        public static string msgOrderIsCancelled {
            get {
                return ResourceManager.GetString("msgOrderIsCancelled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Order was not found..
        /// </summary>
        public static string msgOrderNotFound {
            get {
                return ResourceManager.GetString("msgOrderNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Create/Updating order with number {Number}.
        /// </summary>
        public static string msgOrderSuccess {
            get {
                return ResourceManager.GetString("msgOrderSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Value must be greater than or equal to {1}..
        /// </summary>
        public static string msgQuantity {
            get {
                return ResourceManager.GetString("msgQuantity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Retrieving order with number {Number}.
        /// </summary>
        public static string msgRetrievingOrder {
            get {
                return ResourceManager.GetString("msgRetrievingOrder", resourceCulture);
            }
        }
    }
}
