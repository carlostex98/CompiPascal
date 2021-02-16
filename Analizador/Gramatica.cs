using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;


namespace CompiPascal.Analizador
{
    class Gramatica: Grammar
    {



        public Gramatica()
        {
            
            #region ER
            var numero = new NumberLiteral("numero");
            var identificador = new IdentifierTerminal("identificador");
            var cadena = new StringLiteral("cadena", "\'");
            var comentario_uno= new CommentTerminal("comentario_uno", "//", "\n", "\r\n");
            var comentario_multi = new CommentTerminal("comentario_multi", "(*", "*)");
            var comentario_multi_ = new CommentTerminal("comentario_multi", "{", "}");
            #endregion

            #region Terminales
            
            //operadores aritmeticos
            var mas = ToTerm("+");
            var menos = ToTerm("-");
            var por = ToTerm("*");
            var dividir = ToTerm("/");
            var modulo = ToTerm("%");

            //operadores logicos
            var mayor = ToTerm(">");
            var menor = ToTerm("<");

            //asignacion
            var igual = ToTerm("=");

            //negacion
            var negacion = ToTerm("!");

            //agrupacion
            var pizq = ToTerm("(");
            var pder = ToTerm(")");
            var cizq = ToTerm("[");
            var cder = ToTerm("]");

            //simbolos
            var punto = ToTerm(".");
            var dpunto = ToTerm(":");
            var coma = ToTerm(",");
            var ptcoma = ToTerm(";");

            //palabras clave 
            var begin_ = ToTerm("begin");
            var program_ = ToTerm("program");
            var var_ = ToTerm("var");
            var end_ = ToTerm("end");
            var function_ = ToTerm("function");
            var procedure_ = ToTerm("procedure");
            var then_ = ToTerm("then");
            var else_ = ToTerm("else");
            var if_ = ToTerm("if");
            var writeln_ = ToTerm("writeln");

            var exit_ = ToTerm("Exit");

            var integer_ = ToTerm("integer");
            var string_ = ToTerm("string");
            var boolean_ = ToTerm("boolean");
            var real_ = ToTerm("real");

            var and_ = ToTerm("and");
            var or_ = ToTerm("or");


            RegisterOperators(1, mas, menos);
            RegisterOperators(2, por, dividir);

            #endregion

            #region no-terminales
            NonTerminal asignacion = new NonTerminal("asignacion");
            NonTerminal instrucciones = new NonTerminal("instrucciones");
            NonTerminal parametros = new NonTerminal("parametros");
            NonTerminal print_parametros = new NonTerminal("print_parametros");
            NonTerminal parametros_llamada = new NonTerminal("parametros_llamada");
            NonTerminal if_then = new NonTerminal("if_then");
            NonTerminal funcion = new NonTerminal("funcion");
            NonTerminal procedimiento = new NonTerminal("procedimiento");
            NonTerminal function_call = new NonTerminal("function_call");
            NonTerminal cases = new NonTerminal("cases");
            NonTerminal while_do = new NonTerminal("while_do");
            NonTerminal repeat_until = new NonTerminal("repeat_until");
            NonTerminal for_do = new NonTerminal("for_do");
            NonTerminal var_param = new NonTerminal("var_param"); //pendiente
            NonTerminal expresion = new NonTerminal("expresion");
            NonTerminal valor = new NonTerminal("valor");
            NonTerminal declaracion = new NonTerminal("declaracion");
            NonTerminal main_ = new NonTerminal("main");
            NonTerminal programa = new NonTerminal("programa");
            NonTerminal instr_normal = new NonTerminal("instr_normal");
            NonTerminal print_ = new NonTerminal("print");
            NonTerminal type_var = new NonTerminal("type_var");
            NonTerminal list_vp = new NonTerminal("list_vp");
            

            //NonTerminal  = new NonTerminal("");
            #endregion


            #region reglas 

            instrucciones.Rule 
                = funcion
                | programa
                | declaracion
                | procedimiento
                | main_
                ;

            programa.Rule = program_ + identificador;

            procedimiento.Rule
                = procedure_ + identificador + pizq + pder + begin_ + instr_normal + end_ + ptcoma
                | procedure_ + identificador + pizq + parametros + pder + begin_ + instr_normal + end_ + ptcoma
                ;

            funcion.Rule
                = function_ + identificador + pizq + pder + begin_ + instr_normal + end_ + ptcoma
                | function_ + identificador + pizq + parametros + pder + begin_ + instr_normal + end_ + ptcoma
                ;

            parametros.Rule
                = list_vp + dpunto + type_var + ptcoma + parametros
                | list_vp + dpunto + type_var
                ;

            list_vp.Rule
                = identificador + coma + list_vp
                | identificador
                ;

            type_var.Rule
                = string_
                | integer_
                | boolean_
                | real_
                ;


            main_.Rule = begin_ + instr_normal + end_ + ptcoma;

            instr_normal.Rule
                = if_then
                | cases
                | function_call
                | while_do
                | repeat_until
                | for_do
                | print_
                ;

            print_.Rule = writeln_ + pizq + print_parametros + pder + ptcoma;

            print_parametros.Rule 
                = expresion + print_parametros
                | expresion
                ;

            if_then.Rule = if_ + pizq + expresion + pder + instr_normal;



            expresion.Rule
                = valor + mas + valor
                | valor + menos + valor
                | valor + por + valor
                | valor + dividir + valor
                | valor + modulo + valor
                | valor + and_ + valor
                | valor + or_ + valor
                | negacion + valor 
                | valor
                ;

            valor.Rule
                = numero
                | function_call
                | identificador
                | cadena
                | pizq + expresion + pder
                ;

            asignacion.Rule = identificador + dpunto + igual + expresion;

            function_call.Rule 
                = identificador + pizq + pder
                | identificador + pizq + parametros_llamada+ pder
                ;

            parametros_llamada.Rule
                = expresion + coma + parametros_llamada
                | expresion
                ;





            #endregion



            #region CONFIG
            NonGrammarTerminals.Add(comentario_uno);
            NonGrammarTerminals.Add(comentario_multi);
            NonGrammarTerminals.Add(comentario_multi_);
            this.Root = instrucciones;
            #endregion


        }
    }
}
