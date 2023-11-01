// See https://aka.ms/new-console-template for more information
using NK_API_Test.AgingTets;
using NK_API_Test.SingleApiTests;

Console.WriteLine("Hello, World!");

var test1 = new Test_Record_days();
await test1.Test(100);

var test3 = new Test_Crud_Face();
await test3.TestNbyN(10, 100);

var agingTest = new Test_Aging();
await agingTest.Test(100);