// See https://aka.ms/new-console-template for more information
using NK_API_Test.AgingTets;
using NK_API_Test.SingleApiTests;

Console.WriteLine("Hello, World!");

//var test1 = new Test_Record_days();
//await test1.RecordDays();

//var test2 = new Test_Aging();
//await test2.Tets();


var test3 = new Test_Crud_Face();
await test3.TestNbyN(10, 100);


